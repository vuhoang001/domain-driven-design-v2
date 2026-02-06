using MasterData.Application.Configuration.Commands;
using MasterData.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Infrastructure.Configuration;

public static class CommandsExecutor
{
    public static async Task Execute(ICommand command)
    {
        using var scope = MasterDataCompositionRoot.BeginLifetimeScope();

        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);

        await handler.Handle((dynamic)command, CancellationToken.None);
    }

    public static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        using var scope = MasterDataCompositionRoot.BeginLifetimeScope();

        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));

        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);

        return await handler.Handle((dynamic)command, CancellationToken.None);
    }
}