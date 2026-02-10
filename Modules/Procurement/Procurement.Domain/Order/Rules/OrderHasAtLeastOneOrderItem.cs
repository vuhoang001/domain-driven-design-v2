using BuildingBlocks.Domain;

namespace Procurement.Domain.Order.Rules;

public class OrderHasAtLeastOneOrderItem(Order order) : IBusinessRule
{
    public bool IsBroken()
    {
        return order.OrderItems.Count <= 0;
    }

    public string Message => "Order must have at least one order item";
}