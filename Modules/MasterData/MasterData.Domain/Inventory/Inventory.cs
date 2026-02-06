using BuildingBlocks.Domain;

namespace MasterData.Domain.Inventory;

public class Inventory : Entity
{
    public Guid Id { get; private set; }
    public string ItemName { get; private set; }
    public string? ItemDesc { get; private set; }
    public decimal Price { get; private set; }

    private Inventory()
    {
        Id       = Guid.NewGuid();
        ItemName = "";
    }

    protected Inventory(Guid id, string itemName, string? itemDesc, decimal price)
    {
        Id       = id;
        ItemName = itemName;
        ItemDesc = itemDesc;
        Price    = price;
    }

    public static Inventory Create(Guid id, string itemName, string? itemDesc, decimal price)
    {
        var item = new Inventory(id, itemName, itemDesc, price);

        return item;
    }
}