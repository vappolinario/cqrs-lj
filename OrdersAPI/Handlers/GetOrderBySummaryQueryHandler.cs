using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetOrderBySummaryQueryHandler : IRequestHandler<GetOrderBySummaryQuery, List<OrderSummaryDto>>
{
    private readonly ReadDbContext _context;

    public GetOrderBySummaryQueryHandler(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderSummaryDto>> Handle(GetOrderBySummaryQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders.AsNoTracking().Select(o =>
              new OrderSummaryDto(
                o.Id,
                o.FirstName + " " + o.LastName,
                o.Status,
                o.TotalCost
              )).ToListAsync(cancellationToken);
    }
}
