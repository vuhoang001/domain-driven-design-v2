using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;

namespace Procurement.Infrastructure.Configuration.DataAccess;

public static class DataAccessServiceCollectionExtensions
{
    public static void AddDataAccessModule(this IServiceCollection services,
        string connectionString)
    {
        services.AddScoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));


        services.AddDbContext<ProcurementContext>((_, options) =>
        {
            options.UseSqlServer(connectionString);
            options.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
        });

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<ProcurementContext>());

        var infrastructureAssembly = typeof(ProcurementContext).Assembly;
        var repositoryTypes = infrastructureAssembly.GetTypes()
            .Where(t => t.Name.EndsWith("Repository") &&
                       !t.IsAbstract                  &&
                       !t.IsInterface);

        foreach (var repositoryType in repositoryTypes)
        {
            var interfaceType = repositoryType.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{repositoryType.Name}");

            if (interfaceType != null)
            {
                services.AddScoped(interfaceType, repositoryType);
            }
        }
    }
}

