using BuildingBlocks.Domain;

namespace Procurement.Domain.Order.Rules;

public class QuantityMustBeGreaterThanZero(int quantity) : IBusinessRule
{
    public bool IsBroken() => quantity <= 0;


    public string Message => "Quantity must be greater than 0";
}