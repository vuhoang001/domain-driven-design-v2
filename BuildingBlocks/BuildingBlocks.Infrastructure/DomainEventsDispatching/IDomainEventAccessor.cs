using BuildingBlocks.Domain;

namespace BuildingBlocks.Infrastructure.DomainEventsDispatching;

public interface IDomainEventAccessor
{
    IReadOnlyList<IDomainEvent> GetAllDomainEvents();

    void ClearDomainEvents();
}