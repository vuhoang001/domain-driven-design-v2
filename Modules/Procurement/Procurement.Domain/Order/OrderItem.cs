using BuildingBlocks.Domain;
using Procurement.Domain.Order.Rules;

namespace Procurement.Domain.Order;

public class OrderItem : Entity
{
    public OrderItemId OrderItemId { get; private set; }
    public OrderId OrderId { get; private set; }
    public Guid ItemId { get; private set; }

    public string ItemName { get; private set; }
    public string? ItemDescription { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public decimal GetLineTotal() => UnitPrice * Quantity;

    private OrderItem(OrderItemId orderItemId, OrderId orderId, Guid itemId, string itemName, string? itemDescription,
        decimal unitPrice, int quantity)
    {
        OrderItemId     = orderItemId;
        OrderId         = orderId;
        ItemId          = itemId;
        ItemName        = itemName;
        ItemDescription = itemDescription;
        UnitPrice       = unitPrice;
        Quantity        = quantity;
    }

    public static OrderItem Create(OrderItemId orderItemId, OrderId orderId, Guid itemId, string itemName,
        string? itemDescription,
        decimal unitPrice, int quantity)
    {
        return new OrderItem(orderItemId, orderId, itemId, itemName, itemDescription, unitPrice, quantity);
    }

    public void IncreaseQuantity(int quantity)
    {
        CheckRule(new QuantityMustBeGreaterThanZero(quantity));
        Quantity += quantity;
    }

    public void ChangeQuantity(int quantity)
    {
        CheckRule(new QuantityMustBeGreaterThanZero(quantity));
        Quantity = quantity;
    }

    public void ChangeUnitPrice(decimal unitPrice)
    {
        CheckRule(new UnitPriceMustBeGreaterThanZeroRule(unitPrice));
        UnitPrice = unitPrice;
    }
}