using Microsoft.EntityFrameworkCore;

public class GetOrderByIdQueryHandler
{
    public static async Task<OrderDto?> HandleAsync(GetOrderByIdQuery query, ReadDbContext context)
    {
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == query.OrderId);

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
