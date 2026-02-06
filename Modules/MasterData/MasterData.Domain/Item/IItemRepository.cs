namespace MasterData.Domain.Item;

public interface IItemRepository
{
    Task AddAsync(Item item);
}