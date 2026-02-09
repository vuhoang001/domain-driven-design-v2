using FluentValidation;

namespace MasterData.Application.Features.SampleFeature;

public class CreateItemValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemValidator()
    {
        // RuleFor(x => x.Name)
        //     .NotEmpty()
        //     .Must(code => code.StartsWith("hoang"))
        //     .WithMessage("Code must start with ITM");
    }
}