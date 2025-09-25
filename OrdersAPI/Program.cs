using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(
      builder.Configuration.GetConnectionString("BaseConnection")
      ));


builder.Services.AddScoped<ICommandHandler<CreateOrderCommand, OrderDto>, CreateOrderCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetOrderByIdQuery, OrderDto>, GetOrderByIdQueryHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/api/orders", async (ICommandHandler<CreateOrderCommand, OrderDto> handler, CreateOrderCommand command) =>
    {
        var created = await handler.HandleAsync(command);

        if (created == null)
            return Results.BadRequest("Failed to create order");

        return Results.Created($"/api/orders/{created.iD}", created);
    });

app.MapGet("/api/orders/{id}", async (IQueryHandler<GetOrderByIdQuery, OrderDto> handler, int id) =>
    {
        var order = await handler.HandleAsync(new GetOrderByIdQuery(id));
        if (order == null)
            return Results.NotFound();
        return Results.Ok(order);
    });

app.Run();
