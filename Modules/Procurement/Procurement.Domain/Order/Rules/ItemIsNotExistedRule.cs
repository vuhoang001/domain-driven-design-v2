using BuildingBlocks.Domain;

namespace Procurement.Domain.Order.Rules;

public class ItemMustExistRule(OrderItem? orderItem, Guid itemId) : IBusinessRule
{
    public bool IsBroken()
    {
        return orderItem is null;
    }

    public string Message  => $"Item with ID {itemId} does not exist in this order";
}