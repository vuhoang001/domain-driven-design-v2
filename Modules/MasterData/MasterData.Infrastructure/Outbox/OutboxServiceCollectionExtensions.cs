using BuildingBlocks.Application.Events;
using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Infrastructure.Outbox;

public static class OutboxServiceCollectionExtensions
{
    public static IServiceCollection AddOutbox(this IServiceCollection services,
        BiDictionary<string, Type> domainNotificationsMap)
    {
        services.AddScoped<IOutbox, OutboxAccessor>();

        services.AddSingleton<IDomainEventNotificationMapper>(sp =>
                                                                  new DomainEventNotificationMapper(
                                                                      domainNotificationsMap));

        CheckMappings(domainNotificationsMap);

        return services;
    }

    private static void CheckMappings(BiDictionary<string, Type> domainNotificationsMap)
    {
        var domainEventNotifications = Assemblies.Application
            .GetTypes()
            .Where(x => x.GetInterfaces().Contains(typeof(IDomainEventNotification)))
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