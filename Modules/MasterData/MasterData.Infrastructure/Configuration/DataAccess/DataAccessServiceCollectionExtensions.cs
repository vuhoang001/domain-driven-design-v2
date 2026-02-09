using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Infrastructure.Configuration.DataAccess;

public static class DataAccessServiceCollectionExtensions
{
    public static void AddDataAccessModule(this IServiceCollection services,
        string connectionString)
    {
        services.AddScoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        services.AddDbContext<MasterDataContext>((_, options) => { options.UseSqlServer(connectionString); });

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<MasterDataContext>());

        var infrastructureAssembly = typeof(MasterDataContext).Assembly;
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