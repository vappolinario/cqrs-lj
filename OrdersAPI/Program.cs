using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Host.UseWolverine();

builder.Services.AddDbContext<WriteDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("WriteDbConnection")));
builder.Services.AddDbContext<ReadDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("ReadDbConnection")));
builder.Services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/api/orders", async (CreateOrderCommand command, IMessageBus bus) =>
    {
        try
        {
            var orderCreated = await bus.InvokeAsync<OrderCreatedEvent>(command);
            return Results.Created($"/api/orders/{orderCreated.OrderId}", orderCreated);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(errors);
        }
    });

app.MapGet("/api/orders/{id}", async (int id, IMessageBus bus) =>
    {
        var order = await bus.InvokeAsync<OrderDto>(new GetOrderByIdQuery(id));
        if (order == null)
            return Results.NotFound();
        return Results.Ok(order);
    });

app.MapGet("/api/orders", async (IMessageBus bus) =>
    {
        var summary = await bus.InvokeAsync<List<OrderSummaryDto>>(new GetOrderBySummaryQuery());
        if (summary == null)
            return Results.NotFound();
        return Results.Ok(summary);
    });

app.Run();
