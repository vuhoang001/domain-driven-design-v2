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
            var domainEventNotificationType       = typeof(IDomainEventNotification<>);
            var domainNotificationWithGenericType = domainEventNotificationType.MakeGenericType(domainEvent.GetType());

            try
            {
                var domainNotification =
                    ActivatorUtilities.CreateInstance(serviceProvider,
                                                      domainNotificationWithGenericType,
                                                      domainEvent,
                                                      domainEvent.Id);

                if (domainNotification is not null)
                {
                    domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent> ??
                                                 throw new InvalidOperationException());
                }
            }
            catch (Exception e)
            {
                // ignored
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
}