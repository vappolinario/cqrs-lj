using FluentValidation;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderDto>
{
    private readonly AppDbContext _context;
    private readonly IValidator<CreateOrderCommand> _validator;

    public CreateOrderCommandHandler(AppDbContext context, IValidator<CreateOrderCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<OrderDto> HandleAsync(CreateOrderCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

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
