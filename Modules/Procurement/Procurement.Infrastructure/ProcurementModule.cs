using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Procurement.Application.Configuration.Commands;
using Procurement.Application.Contracts;
using Procurement.Infrastructure.Configuration;

namespace Procurement.Infrastructure;

public class ProcurementModule(IServiceProvider serviceProvider) : IProcurementModule
{
    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        return await CommandsExecutor.Execute(command);
    }

    public async Task ExecuteCommandAsync(ICommand command)
    {
        await CommandsExecutor.Execute(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        using var scope    = serviceProvider.CreateScope();
        var       mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(query);
    }
}