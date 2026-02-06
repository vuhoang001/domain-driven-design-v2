namespace BuildingBlocks.Infrastructure.DomainEventsDispatching;

public class DomainEventNotificationMapper(BiDictionary<string, Type> domainNotificationsMap)
    : IDomainEventNotificationMapper
{
    public string GetName(Type type)
    {
        return domainNotificationsMap.TryGetBySecond(type, out var name) ? name : null;
    }

    public Type GetType(string name)
    {
        return domainNotificationsMap.TryGetByFirst(name, out var type) ? type : null;
    }
}