using Procurement.Application.Contracts;

namespace Procurement.Application.Configuration.Commands;

public interface IProcurementModule
{
    Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

    Task ExecuteCommandAsync(ICommand command);

    Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}