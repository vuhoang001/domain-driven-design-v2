using BuildingBlocks.Infrastructure.Email;
using MasterData.Application.Configuration.Commands;
using MasterData.Infrastructure.Configuration;
using MasterData.Infrastructure.Configuration.DataAccess;
using MasterData.Infrastructure.Configuration.Mediation;
using MasterData.Infrastructure.Configuration.Quartz;
using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Infrastructure;

public static class MasterDataStartup
{
    public static IServiceCollection AddMasterDataServices(
        this IServiceCollection services,
        string connectionString,
        EmailConfiguration emailConfiguration)
    {
        // Data Access
        services.AddDataAccessModule(connectionString);

        // MediatR
        services.AddMediator();

        // Processing (Domain Events, UnitOfWork, Commands)
        services.AddProcessing();

        // Email Configuration
        services.AddSingleton(emailConfiguration);

        // Decorators cho CommandHandlers (sử dụng Scrutor)
        services.AddCommandHandlerDecorators();

        // Quartz Scheduler (every 2 seconds)
        services.AddQuartzScheduler(internalProcessingPoolingInterval: 2000);

        // MasterData Module itself
        services.AddScoped<IMasterDataModule, MasterDataModule>();

        return services;
    }
}

