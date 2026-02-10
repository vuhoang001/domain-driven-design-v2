using BuildingBlocks.Domain;

namespace Procurement.Domain.Order.Rules;

public class ItemNameCannotBeEmptyRule(string itemName) : IBusinessRule
{
    public bool IsBroken() => string.IsNullOrWhiteSpace(itemName);

    public string Message => "Item name cannot be empty";
}
