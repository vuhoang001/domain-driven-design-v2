using BuildingBlocks.Domain;
using Procurement.Domain.Order.Events;
using Procurement.Domain.Order.Rules;

namespace Procurement.Domain.Order;

public class Order : Entity, IAggregateRoot
{
    public OrderId Id { get; private set; }
    public string DocCode { get; private set; }
    public decimal TotalPrice { get; private set; }
    private readonly List<OrderItem> _orderItems = new();

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private Order()
    {
    }

    private Order(OrderId id, string docCode)
    {
        Id      = id;
        DocCode = docCode;
    }

    public static Order Create(OrderId id, string docCode)
    {
        var order = new Order(id, docCode);
        order.AddDomainEvent(new CreateOrderDomainEvent());
        return order;
    }

    public void AddItem(OrderItemId orderItemId, Guid itemId, string itemName,
        string? itemDescription, decimal unitPrice, int quantity)
    {
        // Validate input
        ValidateItemInput(itemName, unitPrice, quantity);

        var existingItem = _orderItems.FirstOrDefault(x => x.ItemId == itemId);
        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            var orderItem = OrderItem.Create(orderItemId, Id, itemId, itemName, itemDescription, unitPrice,
                                             quantity);
            _orderItems.Add(orderItem);
        }

        RecalculateAndValidate();
    }

    public void UpdateItem(Guid itemId, decimal unitPrice, int quantity)
    {
        ValidateItemInput(string.Empty, unitPrice, quantity);

        var existingItem = _orderItems.FirstOrDefault(x => x.ItemId == itemId);
        CheckRule(new ItemMustExistRule(existingItem, itemId));

        existingItem!.ChangeQuantity(quantity);
        existingItem.ChangeUnitPrice(unitPrice);

        RecalculateAndValidate();
    }

    private void ValidateItemInput(string itemName, decimal unitPrice, int quantity)
    {
        CheckRule(new ItemNameCannotBeEmptyRule(itemName));
        CheckRule(new UnitPriceMustBeGreaterThanZeroRule(unitPrice));
        CheckRule(new QuantityMustBeGreaterThanZero(quantity));
    }


    private void RecalculateAndValidate()
    {
        var total = _orderItems.Sum(item => item.GetLineTotal());
        TotalPrice = total;
        ValidateOrderState();
    }

    private void ValidateOrderState()
    {
        CheckRule(new OrderHasAtLeastOneOrderItem(this));
        CheckRule(new TotalPriceMustBeGreaterThanZero(this.TotalPrice));
    }
}