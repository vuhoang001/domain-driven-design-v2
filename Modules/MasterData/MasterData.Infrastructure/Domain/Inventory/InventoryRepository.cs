using MasterData.Domain.Inventory;

namespace MasterData.Infrastructure.Domain.Inventory;

public class InventoryRepository(MasterDataContext context) : IInventoryRepository
{
    public async Task AddAsync(MasterData.Domain.Inventory.Inventory inventory)
    {
        await context.Inventories.AddAsync(inventory);
    }
}