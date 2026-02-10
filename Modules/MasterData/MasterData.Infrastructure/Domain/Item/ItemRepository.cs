using MasterData.Domain.Item;
using Microsoft.EntityFrameworkCore;

namespace MasterData.Infrastructure.Domain.Item;

public class ItemRepository(MasterDataContext context) : IItemRepository
{
    public async Task AddAsync(MasterData.Domain.Item.Item item)
    {
        await context.Items.AddAsync(item);
    }

    public void Update(MasterData.Domain.Item.Item item)
    {
        context.Items.Update(item);
    }

    public async Task<MasterData.Domain.Item.Item?> GetByIdAsync(Guid itemId)
    {
        var result = await context.Items.FirstOrDefaultAsync(x => x.Id == new ItemId(itemId));

        return result;
    }
}