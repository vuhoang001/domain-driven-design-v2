using BuildingBlocks.Application;
using MasterData.Application.Configuration.Commands;
using MasterData.Application.Contracts;
using MasterData.Infrastructure.Configuration.DataAccess;

namespace MasterData.Infrastructure.Configuration;

public class ValidationCommandHandlerDecorator<T>(
    IValidatorResolver<T> validatorResolver,
    ICommandHandler<T> decorated) : ICommandHandler<T> where T : ICommand
{
    public async Task Handle(T request, CancellationToken cancellationToken)
    {
        var validators = validatorResolver.GetValidators();
        
        if (validators.Count > 0)
        {
            var errors = validators.Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (errors.Count > 0)
            {
                throw new InvalidCommandException(errors.Select(x => x.ErrorMessage).ToList());
            }
        }

        await decorated.Handle(request, cancellationToken);
    }
}