using BuildingBlocks.Infrastructure;
using Procurement.Application.Configuration.Commands;
using Procurement.Application.Contracts;

namespace Procurement.Infrastructure.Configuration;

internal class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult>(
    ICommandHandler<T, TResult> decorated,
    IUnitOfWork unitOfWork,
    ProcurementContext masterDataContext) : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
{
    public async Task<TResult> Handle(T request, CancellationToken cancellationToken)
    {
        var result = await decorated.Handle(request, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);
        return result;
    }
}