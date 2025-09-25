public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderDto>
{
    private readonly AppDbContext _context;


    public CreateOrderCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDto> HandleAsync(CreateOrderCommand command)
    {
        var order = new Order
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Status = command.Status,
            CreatedAt = DateTime.Now,
            TotalCost = command.TotalCost,
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        return new OrderDto(
           order.Id,
           order.FirstName,
           order.LastName,
           order.Status,
           order.CreatedAt,
           order.TotalCost
           );
    }
}
