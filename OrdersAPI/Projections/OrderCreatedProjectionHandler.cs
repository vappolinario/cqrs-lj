using Wolverine.Attributes;

public class OrderCreatedProjectionHandler
{
    [Transactional]
    public static void Handle(OrderCreatedEvent evt, ReadDbContext context)
    {
        var order = new Order
        {
            Id = evt.OrderId,
            FirstName = evt.FirstName,
            LastName = evt.LastName,
            Status = "Created",
            CreatedAt = DateTime.Now,
            TotalCost = evt.TotalCost
        };

        context.Orders.Add(order);
        context.SaveChanges();

        Console.WriteLine($"Replicou {evt.OrderId}");
    }
}
