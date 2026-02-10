using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Microsoft.Extensions.DependencyInjection;

namespace Procurement.Infrastructure.Configuration;

public static class ProcessingServiceCollectionExtensions
{
    public static IServiceCollection AddProcessing(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IDomainEventAccessor, DomainEventAccessor>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        return services;
    }
}