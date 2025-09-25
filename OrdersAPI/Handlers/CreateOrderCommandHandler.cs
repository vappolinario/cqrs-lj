public class CreateOrderCommandHandler
{
  public static async Task<Order> Handle(CreateOrderCommands command, AppDbContext context)
  {
    var order = new Order
    {
      FirstName = command.FirstName,
      LastName = command.LastName,
      Status = command.Status,
      CreatedAt = DateTime.Now,
      TotalCost = command.TotalCost,
    };

    await context.Orders.AddAsync(order);
    await context.SaveChangesAsync();

    return order;
  }

}
