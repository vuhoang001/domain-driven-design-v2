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

        services.AddOutbox();

        return services;
    }
}