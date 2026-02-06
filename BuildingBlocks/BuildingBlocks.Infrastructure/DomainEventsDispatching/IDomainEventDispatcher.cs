namespace BuildingBlocks.Infrastructure.DomainEventsDispatching;

public interface IDomainEventDispatcher
{
    Task DispatchEventAsync();
}