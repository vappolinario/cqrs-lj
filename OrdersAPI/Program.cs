using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(
      builder.Configuration.GetConnectionString("BaseConnection")
      ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/api/orders", async (AppDbContext context, CreateOrderCommands command) =>
    {
        var created = await CreateOrderCommandHandler.Handle(command, context);

        if (created == null)
            return Results.BadRequest("Failed to create order");

        return Results.Created($"/api/orders/{created.Id}", created);
    });

app.MapGet("/api/orders/{id}", async (AppDbContext context, int id) =>
    {
        var order = await GetOrderByIdQueryHandler.Handle(new GetOrderByIdQuery(id), context);
        if (order == null)
            return Results.NotFound();
        return Results.Ok(order);
    });

app.Run();
