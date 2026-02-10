using Procurement.Application.Configuration.Commands;
using Procurement.Domain.Order;

namespace Procurement.Application.Features.Order;

public class CreateOrderCommandHandler(IOrderRepository orderRepository) : ICommandHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = new OrderId(Guid.NewGuid());
        var order   = Domain.Order.Order.Create(orderId, request.DocCode);

        foreach (var items in request.Items)
        {
            order.AddItem(new OrderItemId(Guid.NewGuid()), items.ItemId, items.ItemName, items.ItemDescription,
                          items.UnitPrice,
                          items.Quantity);
        }

        await orderRepository.AddAsync(order);
        return orderId.Value;
    }
}