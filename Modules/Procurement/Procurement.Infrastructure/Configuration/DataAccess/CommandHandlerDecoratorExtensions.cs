using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Procurement.Application.Configuration.Commands;

namespace Procurement.Infrastructure.Configuration.DataAccess;

public static class CommandHandlerDecoratorsExtensions
{
    public static void AddCommandHandlerDecorators(this IServiceCollection services)
    {
        var assembly = typeof(CommandHandlerDecoratorsExtensions).Assembly;

        var appAssembly = typeof(ICommandHandler<>).Assembly;

        services.AddValidatorsFromAssembly(appAssembly);

        services.AddTransient(typeof(IValidatorResolver<>), typeof(ValidatorResolver<>));

        DecorateIfExists(services, assembly, typeof(ICommandHandler<>),
                         "UnitOfWorkCommandHandlerDecorator", 1);
        DecorateIfExists(services, assembly, typeof(ICommandHandler<,>),
                         "UnitOfWorkCommandHandlerWithResultDecorator", 2);

        DecorateIfExists(services, assembly, typeof(ICommandHandler<>),
                         "ValidationCommandHandlerDecorator", 1);
        DecorateIfExists(services, assembly, typeof(ICommandHandler<,>),
                         "ValidationCommandHandlerWithResultDecorator", 2);
    }

    private static void DecorateIfExists(
        IServiceCollection services,
        Assembly assembly,
        Type serviceType,
        string decoratorTypeName,
        int genericParameterCount)
    {
        var decoratorType = assembly.GetTypes()
            .FirstOrDefault(t =>
                                t.Name == $"{decoratorTypeName}`{genericParameterCount}" &&
                                t.IsGenericTypeDefinition                                &&
                                t.GetGenericArguments().Length == genericParameterCount);

        if (decoratorType == null)
        {
            return;
        }

        try
        {
            services.Decorate(serviceType, decoratorType);
        }
        catch (Exception ex)
        {
            // ignored
        }
    }
}

public interface IValidatorResolver<T>
{
    IList<IValidator<T>> GetValidators();
}

public class ValidatorResolver<T>(IServiceProvider serviceProvider) : IValidatorResolver<T>
{
    public IList<IValidator<T>> GetValidators()
    {
        return serviceProvider.GetServices(typeof(IValidator<T>))
            .Cast<IValidator<T>>()
            .ToList();
    }
}