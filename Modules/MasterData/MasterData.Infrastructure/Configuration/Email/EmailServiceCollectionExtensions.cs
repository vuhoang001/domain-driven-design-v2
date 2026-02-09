using BuildingBlocks.Application.Email;
using BuildingBlocks.Infrastructure.Email;
using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Infrastructure.Configuration.Email;

public static class EmailServiceCollectionExtensions
{
    public static void AddEmail(this IServiceCollection services)
    {
        services.AddScoped<IEmailSender, EmailSender>();
    }
}