using BuildingBlocks.Application.Events;
using MasterData.Domain.Item.Events;

namespace MasterData.Application.Features.SampleFeature;

public class CreateItemNotification(CreateItemDomainEvent domainEvent)
    : DomainEventNotification<CreateItemDomainEvent>(domainEvent, Guid.NewGuid())
{
}

