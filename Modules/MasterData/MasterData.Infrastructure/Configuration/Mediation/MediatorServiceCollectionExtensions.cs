using MasterData.Application.Configuration.Commands;
using MasterData.Application.Configuration.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Infrastructure.Configuration.Mediation;

public static class MediatorServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        var applicationAssembly    = Assemblies.Application;
        var infrastructureAssembly = Assemblies.Infrastructure;

        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(applicationAssembly); });

        // Scan cả Application và Infrastructure
        var assemblies = new[] { applicationAssembly, infrastructureAssembly };

        services.Scan(scan => scan
                          .FromAssemblies(assemblies)
                          .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
                          .AsImplementedInterfaces()
                          .WithScopedLifetime());

        services.Scan(scan => scan
                          .FromAssemblies(assemblies)
                          .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                          .AsImplementedInterfaces()
                          .WithScopedLifetime());

        services.Scan(scan => scan
                          .FromAssemblies(assemblies)
                          .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                          .AsImplementedInterfaces()
                          .WithScopedLifetime());

        // ✅ Register IDomainEventNotification handlers với custom factory
        // Tương tự Autofac: .AsClosedTypesOf(typeof(IDomainEventNotification<>))
        services.AddDomainEventNotifications(assemblies);

        return services;
    }
}