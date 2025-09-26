using MediatR;
public record GetOrderBySummaryQuery() : IRequest<List<OrderSummaryDto>>;
