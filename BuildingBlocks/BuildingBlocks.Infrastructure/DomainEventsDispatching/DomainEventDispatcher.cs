using BuildingBlocks.Application.Events;
using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Domain;
using BuildingBlocks.Infrastructure.Serialization;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BuildingBlocks.Infrastructure.DomainEventsDispatching;

public class DomainEventDispatcher(
    IMediator mediator,
    IServiceProvider serviceProvider,
    IOutbox outbox,
    IDomainEventAccessor domainEventAccessor,
    IDomainEventNotificationMapper domainEventNotificationMapper
)
    : IDomainEventDispatcher
{
    public async Task DispatchEventAsync()
    {
        var                                          domainEvents = domainEventAccessor.GetAllDomainEvents();
        List<IDomainEventNotification<IDomainEvent>> domainEventNotifications = [];

        foreach (var domainEvent in domainEvents)
        {
            var concreteNotificationType = FindConcreteNotificationType(domainEvent.GetType());

            if (concreteNotificationType != null)
            {
                try
                {
                    var domainNotification = Activator.CreateInstance(
                        concreteNotificationType,
                        domainEvent,
                        domainEvent.Id
                    );

                    if (domainNotification != null)
                    {
                        domainEventNotifications.Add(
                            (IDomainEventNotification<IDomainEvent>)domainNotification
                        );
                    }
                }
                catch (Exception e)
                {
                    // Log error
                    throw new InvalidOperationException(
                        $"Failed to create notification for {domainEvent.GetType().Name}", e);
                }
            }
        }

        domainEventAccessor.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        foreach (var domainEventNotification in domainEventNotifications)
        {
            var type = domainEventNotificationMapper.GetName(domainEventNotification.GetType());
            var data = JsonConvert.SerializeObject(domainEventNotification, new JsonSerializerSettings()
            {
                ContractResolver = new AllPropertiesContractResolver()
            });

            var outboxMessage = new OutboxMessage(
                domainEventNotification.Id,
                domainEventNotification.DomainEvent.OccurredOn,
                type,
                data);

            outbox.Add(outboxMessage);
        }
    }

    private Type? FindConcreteNotificationType(Type domainEventType)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            try
            {
                var notificationType = assembly.GetTypes()
                    .FirstOrDefault(t =>
                                        !t.IsAbstract                                                              &&
                                        !t.IsInterface                                                             &&
                                        t.BaseType?.IsGenericType             == true                              &&
                                        t.BaseType.GetGenericTypeDefinition() == typeof(DomainEventNotification<>) &&
                                        t.BaseType.GetGenericArguments()[0]   == domainEventType
                    );

                if (notificationType != null)
                    return notificationType;
            }
            catch
            {
                continue;
            }
        }

        return null;
    }
}