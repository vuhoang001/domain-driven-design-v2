using MasterData.Application.Configuration.Commands;
using MasterData.Domain.Inventory;
using MasterData.Domain.Item;

namespace MasterData.Application.Features.SampleFeature;

public class CreateItemCommandHandler(
    IItemRepository itemRepository,
    IInventoryRepository inventoryRepository
)
    : ICommandHandler<CreateItemCommand, string>
{
    public async Task<string> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        // ✅ 1. Tạo Item
        var item = Item.Create(new ItemId(Guid.NewGuid()), command.Name, "Sample Description", 10);
        await itemRepository.AddAsync(item);

        return $"Item '{command.Name}' + Inventory created in same transaction";
    }
}