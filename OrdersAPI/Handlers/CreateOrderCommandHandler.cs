using FluentValidation;
using Wolverine;
using Wolverine.Attributes;

public class CreateOrderCommandHandler
{
    [Transactional]
    public static OrderCreatedEvent Handle(CreateOrderCommand command, WriteDbContext context, IValidator<CreateOrderCommand> validator, IMessageBus bus)
    {
        var validationResult = validator.Validate(command);
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

        context.Orders.Add(order);
        context.SaveChanges();

        var orderCreatedEvent = new OrderCreatedEvent(
              order.Id,
              order.FirstName,
              order.LastName,
              order.TotalCost
            );

        bus.SendAsync(orderCreatedEvent);

        return orderCreatedEvent;
    }
}
