using BuildingBlocks.Application.Email;
using Microsoft.Extensions.DependencyInjection;
using Procurement.Application.Configuration.Commands;
using Procurement.Infrastructure.Configuration;
using Procurement.Infrastructure.Configuration.DataAccess;
using Procurement.Infrastructure.Configuration.Mediation;

namespace Procurement.Infrastructure;

public static class ProcurementStartup
{
    public static void AddProcurementProcurementServices(
        this IServiceCollection services, string connectionString, EmailConfiguration emailConfiguration)
    {
        // Data Access
        services.AddDataAccessModule(connectionString);

        services.AddMediator();

        services.AddProcessing();

        services.AddCommandHandlerDecorators();

        services.AddScoped<IProcurementModule, ProcurementModule>();
    }
}