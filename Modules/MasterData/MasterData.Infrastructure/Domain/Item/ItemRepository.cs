using MasterData.Domain.Inventory;
using MasterData.Domain.Item;

namespace MasterData.Infrastructure.Domain.Item;

public class ItemRepository(MasterDataContext context) : IItemRepository
{
    public async Task AddAsync(MasterData.Domain.Item.Item item)
    {
        await context.Items.AddAsync(item);
    }
}