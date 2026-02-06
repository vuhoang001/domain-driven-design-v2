using Microsoft.OpenApi.Models;

namespace ddd.API.Extensions;

internal static class SwaggerExtensions
{
    internal static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "ddd API", Version = "v1" });
            options.CustomSchemaIds(t => t.ToString());
        });
    }

    internal static void UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ddd API v1");
        });
    }
}

