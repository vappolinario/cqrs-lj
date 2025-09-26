using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<WriteDbContext>(opt => opt.UseSqlite(
      builder.Configuration.GetConnectionString("WriteDbConnection")
      ));

builder.Services.AddDbContext<ReadDbContext>(opt => opt.UseSqlite(
      builder.Configuration.GetConnectionString("ReadDbConnection")
      ));

builder.Services.AddScoped<ICommandHandler<CreateOrderCommand, OrderDto>, CreateOrderCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetOrderByIdQuery, OrderDto>, GetOrderByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetOrderBySummaryQuery, List<OrderSummaryDto>>, GetOrderBySummaryQueryHandler>();
builder.Services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
builder.Services.AddSingleton<IEventPublisher, ConsoleEventPublisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/api/orders", async (ICommandHandler<CreateOrderCommand, OrderDto> handler, CreateOrderCommand command) =>
    {
        try
        {
            var created = await handler.HandleAsync(command);
            if (created == null)
                return Results.BadRequest("Failed to create order");

            return Results.Created($"/api/orders/{created.iD}", created);
        }
        catch (ValidationException ex)
        {
            var erros = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(erros);
        }
    });

app.MapGet("/api/orders/{id}", async (IQueryHandler<GetOrderByIdQuery, OrderDto> handler, int id) =>
    {
        var order = await handler.HandleAsync(new GetOrderByIdQuery(id));
        if (order == null)
            return Results.NotFound();
        return Results.Ok(order);
    });

app.MapGet("/api/orders", async (IQueryHandler<GetOrderBySummaryQuery, List<OrderSummaryDto>> handler) =>
    {
        var summary = await handler.HandleAsync(new GetOrderBySummaryQuery());
        if (summary == null)
            return Results.NotFound();
        return Results.Ok(summary);
    });

app.Run();
