using BuildingBlocks.Infrastructure;
using MasterData.Application.Configuration.Commands;
using MasterData.Application.Contracts;

namespace MasterData.Infrastructure.Configuration;

internal class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult>(
    ICommandHandler<T, TResult> decorated,
    IUnitOfWork unitOfWork,
    MasterDataContext masterDataContext) : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
{
    public async Task<TResult> Handle(T request, CancellationToken cancellationToken)
    {
        var result = await decorated.Handle(request, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);
        return result;
    }
}