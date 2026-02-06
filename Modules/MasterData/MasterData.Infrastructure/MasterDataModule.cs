using MasterData.Application.Configuration.Commands;
using MasterData.Application.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Infrastructure;

/// <summary>
/// MasterData Module - executes commands and queries via MediatR.
/// </summary>
public class MasterDataModule : IMasterDataModule
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="MasterDataModule"/> class.
    /// </summary>
    /// <param name="serviceProvider">Main service provider from DI container.</param>
    public MasterDataModule(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Execute command that returns a result.
    /// </summary>
    /// <typeparam name="TResult">Result type.</typeparam>
    /// <param name="command">Command to execute.</param>
    /// <returns>Command result.</returns>
    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        using var scope    = _serviceProvider.CreateScope();
        var       mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(command);
    }

    /// <summary>
    /// Execute command without return value.
    /// </summary>
    /// <param name="command">Command to execute.</param>
    /// <returns>Task.</returns>
    public async Task ExecuteCommandAsync(ICommand command)
    {
        using var scope    = _serviceProvider.CreateScope();
        var       mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(command);
    }

    /// <summary>
    /// Execute query that returns a result.
    /// </summary>
    /// <typeparam name="TResult">Result type.</typeparam>
    /// <param name="query">Query to execute.</param>
    /// <returns>Query result.</returns>
    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        using var scope    = _serviceProvider.CreateScope();
        var       mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(query);
    }
}