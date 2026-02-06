using MasterData.Domain.Inventory;
using MediatR;

namespace MasterData.Application.Features.SampleFeature;

public class CreateItemNotificationHandler(IInventoryRepository inventoryRepository)
    : INotificationHandler<CreateItemNotification>
{
    public async Task Handle(CreateItemNotification notification, CancellationToken cancellationToken)
    {
        var inventory = Domain.Inventory.Inventory.Create(Guid.NewGuid(), "inventoryname", "inventorydesc", 10);

        await inventoryRepository.AddAsync(inventory);
    }
}