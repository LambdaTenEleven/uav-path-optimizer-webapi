using FluentValidation;
using MediatR;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;

public sealed class UpdateUavModelCommandValidator : AbstractValidator<UpdateUavModelCommand>
{
    public UpdateUavModelCommandValidator(IMediator mediator)
    {
        RuleFor(x => x.Id)
            .NotNull();

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.MaxSpeed)
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.MaxFlightTime)
            .NotNull()
            .GreaterThan(TimeSpan.Zero);

        // TODO: Add validation rules for UavModel properties.
    }
}