namespace Procurement.Application.Features.Order;

public class CreateOrderItemDto
{
    public Guid ItemId { get; init; }
    public string ItemName { get; init; } = default!;
    public string? ItemDescription { get; init; }
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; } 
}