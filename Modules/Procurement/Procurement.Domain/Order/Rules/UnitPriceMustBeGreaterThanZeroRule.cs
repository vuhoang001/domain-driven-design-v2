using BuildingBlocks.Domain;

namespace Procurement.Domain.Order.Rules;

public class UnitPriceMustBeGreaterThanZeroRule(decimal unitPrice) : IBusinessRule
{
    public bool IsBroken() => unitPrice <= 0;

    public string Message => "Unit price must be greater than 0";
}
