
public class OrderCreatedConsoleHandler
{
    public Task HandleAsync(OrderCreatedEvent evt)
    {
        Console.WriteLine($"Event published: {evt}");
        return Task.CompletedTask;
    }
}
