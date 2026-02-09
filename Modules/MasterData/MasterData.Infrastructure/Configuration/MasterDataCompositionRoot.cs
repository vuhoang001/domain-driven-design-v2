using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Infrastructure.Configuration;

public static class MasterDataCompositionRoot
{
    private static IServiceProvider? _serviceProvider;

    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static IServiceScope BeginLifetimeScope()
    {
        if (_serviceProvider == null)
            throw new InvalidOperationException(
                "ServiceProvider has not been initialized. Call SetServiceProvider first.");
        return _serviceProvider.CreateScope();
    }
}