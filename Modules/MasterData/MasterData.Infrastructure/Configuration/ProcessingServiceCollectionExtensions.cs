using BuildingBlocks.Application.Events;
using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using MasterData.Infrastructure.Outbox;
using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Infrastructure.Configuration;

public static class ProcessingServiceCollectionExtensions
{
    public static IServiceCollection AddProcessing(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IDomainEventAccessor, DomainEventAccessor>();
        services.AddScoped<IOutbox, OutboxAccessor>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        var assembly = Assemblies.Application;

        var domainNotificationsMap = new BiDictionary<string, Type>();
        var notificationTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                       .Any(i => i.IsGenericType &&
                                i.GetGenericTypeDefinition() == typeof(IDomainEventNotification<>)))
            .Where(t => !t.IsAbstract && !t.IsInterface);

        foreach (var type in notificationTypes)
        {
            var interfaceType = type.GetInterfaces()
                .First(i => i.IsGenericType &&
                           i.GetGenericTypeDefinition() == typeof(IDomainEventNotification<>));
            services.AddTransient(interfaceType, type);

            domainNotificationsMap.Add(type.FullName ?? type.Name, type);
        }

        services.AddOutbox(domainNotificationsMap);

        return services;
    }
}