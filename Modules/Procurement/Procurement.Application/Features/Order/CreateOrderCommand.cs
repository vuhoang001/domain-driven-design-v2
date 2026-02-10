using Procurement.Application.Contracts;

namespace Procurement.Application.Features.Order;

public class CreateOrderCommand : CommandBase<Guid>
{
    public Guid OrderId { get; init; }
    public string DocCode { get; init; } = default!;

    public List<CreateOrderItemDto> Items { get; init; } = new();
}