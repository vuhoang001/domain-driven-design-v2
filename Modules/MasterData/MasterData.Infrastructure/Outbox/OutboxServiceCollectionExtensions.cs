using BuildingBlocks.Application.Events;
using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Infrastructure.Outbox;

public static class OutboxServiceCollectionExtensions
{
    public static void AddOutbox(this IServiceCollection services,
        BiDictionary<string, Type>? domainNotificationsMap = null)
    {
        if (domainNotificationsMap == null)
        {
            domainNotificationsMap = BuildDomainNotificationsMap();
        }

        services.AddScoped<IOutbox, OutboxAccessor>();

        services.AddSingleton<IDomainEventNotificationMapper>(sp =>
                                                                  new DomainEventNotificationMapper(
                                                                      domainNotificationsMap));

        CheckMappings(domainNotificationsMap);
    }

    /// <summary>
    /// Auto-discover and build mapping for IDomainEventNotification handlers
    /// </summary>
    private static BiDictionary<string, Type> BuildDomainNotificationsMap()
    {
        var map = new BiDictionary<string, Type>();

        var domainEventNotifications = Assemblies.Application
            .GetTypes()
            .Where(x => x.GetInterfaces().Any(i =>
                                                  i.IsGenericType &&
                                                  i.GetGenericTypeDefinition() == typeof(IDomainEventNotification<>)))
            .ToList();

        foreach (var notificationType in domainEventNotifications)
        {
            // Use fully qualified name as key
            var key = notificationType.FullName ?? notificationType.Name;
            map.Add(key, notificationType);
        }

        return map;
    }

    private static void CheckMappings(BiDictionary<string, Type> domainNotificationsMap)
    {
        var domainEventNotifications = Assemblies.Application
            .GetTypes()
            .Where(x => x.GetInterfaces().Any(i =>
                                                  i.IsGenericType &&
                                                  i.GetGenericTypeDefinition() == typeof(IDomainEventNotification<>)))
            .ToList();

        List<Type> notMappedNotifications = [];
        foreach (var domainEventNotification in domainEventNotifications)
        {
            domainNotificationsMap.TryGetBySecond(domainEventNotification, out var name);

            if (name == null)
            {
                notMappedNotifications.Add(domainEventNotification);
            }
        }

        if (notMappedNotifications.Any())
        {
            throw new ApplicationException(
                $"Domain Event Notifications {notMappedNotifications.Select(x => x.FullName).Aggregate((x, y) => x + "," + y)} not mapped");
        }
    }
}