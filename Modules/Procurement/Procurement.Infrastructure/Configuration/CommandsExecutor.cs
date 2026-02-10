using Microsoft.Extensions.DependencyInjection;
using Procurement.Application.Configuration.Commands;
using Procurement.Application.Contracts;

namespace Procurement.Infrastructure.Configuration;

public static class CommandsExecutor
{
    public static async Task Execute(ICommand command)
    {
        using var scope = ProcurementCompositionRoot.BeginLifetimeScope();

        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);

        await handler.Handle((dynamic)command, CancellationToken.None);
    }

    public static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        using var scope = ProcurementCompositionRoot.BeginLifetimeScope();

        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));

        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);

        return await handler.Handle((dynamic)command, CancellationToken.None);
    }
}