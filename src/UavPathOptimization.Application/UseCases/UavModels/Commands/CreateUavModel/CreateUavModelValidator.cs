using FluentValidation;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.CreateUavModel;

public sealed class CreateUavModelValidator : AbstractValidator<CreateUavModelCommand>
{
    public CreateUavModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.MaxSpeed)
            .GreaterThan(0);

        RuleFor(x => x.MaxFlightTime)
            .GreaterThan(TimeSpan.Zero);
    }
}