namespace MasterData.Domain.Inventory;

public interface IInventoryRepository
{
    Task AddAsync(Inventory inventory);
}