using BuildingBlocks.Domain;

namespace MasterData.Domain.Item.Events;

public class CreateItemDomainEvent(Guid itemId, string itemName, string? itemDesc, decimal price)
    : DomainEventBase
{
    public Guid ItemId { get; } = itemId;
    public string ItemName { get; } = itemName;
    public string? ItemDesc { get; } = itemDesc;
    public decimal Price { get; } = price;
}