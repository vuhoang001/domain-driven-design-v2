using System.Reflection.Metadata;
using BuildingBlocks.Domain;

namespace Procurement.Domain.Order.Rules;

public class TotalPriceMustBeGreaterThanZero(decimal totalPrice) : IBusinessRule
{
    public bool IsBroken() => totalPrice <= 0;


    public string Message => "Total price must be greater than 0";
}