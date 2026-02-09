using BuildingBlocks.Domain;
using MasterData.Domain.Item.Events;
using MasterData.Domain.Item.Rules;

namespace MasterData.Domain.Item;

public class Item : Entity, IAggregateRoot
{
    public Guid Id { get; private set; }
    public string ItemName { get; private set; }
    public string? ItemDesc { get; private set; }
    public decimal Price { get; private set; }

    private Item()
    {
        Id       = Guid.NewGuid();
        ItemName = "";
    }

    protected Item(Guid id, string itemName, string? itemDesc, decimal price)
    {
        CheckRule(new PriceMustBeGreaterThanZeroRule(price));
        Id       = id;
        ItemName = itemName;
        ItemDesc = itemDesc;
        Price    = price;
    }

    public static Item Create(Guid id, string itemName, string? itemDesc, decimal price)
    {
        var item = new Item(id, itemName, itemDesc, price);

        item.AddDomainEvent(new CreateItemDomainEvent(id, itemName, itemDesc, price));

        return item;
    }

    public void Update(string itemName, string? itemDesc, decimal price)
    {
        ItemName = itemName;
        ItemDesc = itemDesc;
        Price    = price;
    }
}