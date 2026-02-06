using MasterData.Domain.Inventory;
using MediatR;

namespace MasterData.Application.Features.SampleFeature;

public class CreateItemNotificationHandler(IInventoryRepository inventoryRepository)
    : INotificationHandler<CreateItemNotification>
{
    public async Task Handle(CreateItemNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var inventory =
            Domain.Inventory.Inventory.Create(domainEvent.ItemId, $"Inventory for {domainEvent.ItemName}",
                                              "Auto-created", 0);

        await inventoryRepository.AddAsync(inventory);
    }
}