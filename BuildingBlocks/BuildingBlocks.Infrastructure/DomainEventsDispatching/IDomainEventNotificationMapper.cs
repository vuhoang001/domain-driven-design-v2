namespace BuildingBlocks.Infrastructure.DomainEventsDispatching;

public interface IDomainEventNotificationMapper
{
    string GetName(Type type);

    Type GetType(string name);
}