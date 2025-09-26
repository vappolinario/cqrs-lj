
using Microsoft.EntityFrameworkCore;

public class GetOrderBySummaryQueryHandler
{
    public static async Task<List<OrderSummaryDto>?> HandleAsync(GetOrderBySummaryQuery query, ReadDbContext context)
    {
        return await context.Orders.Select(o =>
              new OrderSummaryDto(
                o.Id,
                o.FirstName + " " + o.LastName,
                o.Status,
                o.TotalCost
              )).ToListAsync();
    }
}
