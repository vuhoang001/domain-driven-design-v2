using BuildingBlocks.Domain;

namespace MasterData.Domain.Item.Rules;

public class PriceMustBeGreaterThanZeroRule(decimal price) : IBusinessRule
{
    public bool IsBroken()
    {
        return price <= 0;
    }

    public string Message => "Price must be greater than 0";
}