using BuildingBlocks.Application.Events;
using MasterData.Domain.Item.Events;

namespace MasterData.Application.Features.SampleFeature;

public class CreateItemNotification : IDomainEventNotification<CreateItemDomainEvent>
{
    public Guid Id { get; } = Guid.NewGuid();
    public CreateItemDomainEvent DomainEvent { get; set; } = null!;

    public async Task Handle(CreateItemDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        DomainEvent = domainEvent;
        await Task.CompletedTask;
    }
}
