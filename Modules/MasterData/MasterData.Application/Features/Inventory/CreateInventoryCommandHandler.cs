using MasterData.Application.Configuration.Commands;
using MasterData.Domain.Inventory;

namespace MasterData.Application.Features.Inventory;

public class CreateInventoryCommandHandler(IInventoryRepository inventoryRepository)
    : ICommandHandler<CreateInventoryCommand>
{
    public Task Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
    {
        var inventory = Domain.Inventory.Inventory.Create(Guid.NewGuid(), "inventory", "desc", 0);

        return inventoryRepository.AddAsync(inventory);
    }
}