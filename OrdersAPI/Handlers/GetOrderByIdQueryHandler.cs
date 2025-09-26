using Microsoft.EntityFrameworkCore;

public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly ReadDbContext _context;

    public GetOrderByIdQueryHandler(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDto?> HandleAsync(GetOrderByIdQuery query)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == query.OrderId);

        if (order == null)
            return null;

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
