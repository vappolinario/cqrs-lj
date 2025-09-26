
using Microsoft.EntityFrameworkCore;

public class GetOrderBySummaryQueryHandler : IQueryHandler<GetOrderBySummaryQuery, List<OrderSummaryDto>>
{
    private readonly ReadDbContext _context;

    public GetOrderBySummaryQueryHandler(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderSummaryDto>?> HandleAsync(GetOrderBySummaryQuery query)
    {
        return await _context.Orders.Select(o =>
              new OrderSummaryDto(
                o.Id,
                o.FirstName + " " + o.LastName,
                o.Status,
                o.TotalCost
              )).ToListAsync();
    }
}
