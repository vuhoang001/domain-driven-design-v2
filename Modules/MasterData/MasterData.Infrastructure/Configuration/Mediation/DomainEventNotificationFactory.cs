using BuildingBlocks.Application.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MasterData.Infrastructure.Configuration.Mediation;

public static class DomainEventNotificationFactory
{
    public static IServiceCollection AddDomainEventNotifications(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var notificationTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && !t.IsInterface &&
                       t.GetInterfaces().Any(i =>
                                                 i.IsGenericType &&
                                                 i.GetGenericTypeDefinition() == typeof(IDomainEventNotification<>)))
            .ToList();

        foreach (var notificationType in notificationTypes)
        {
            var notificationInterface = notificationType
                .GetInterfaces()
                .FirstOrDefault(i =>
                                    i.IsGenericType &&
                                    i.GetGenericTypeDefinition() == typeof(IDomainEventNotification<>));

            if (notificationInterface != null)
            {
                // ✅ Register trực tiếp - notification handlers có parameterless constructor
                // Event sẽ được truyền qua Handle() method từ DomainEventDispatcher
                services.AddTransient(notificationInterface, notificationType);
            }
        }

        return services;
    }
}

