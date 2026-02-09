namespace MasterData.Domain.Item;

public interface IItemRepository
{
    Task AddAsync(Item item);
    void Update(Item item);

    Task<Item?> GetByIdAsync(Guid itemId);
}