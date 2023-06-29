using FluentValidation;
using MediatR;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;

public class UpdateUavModelCommandValidator : AbstractValidator<UpdateUavModelCommand>
{
    private readonly IMediator _mediator;

    public UpdateUavModelCommandValidator(IMediator mediator)
    {
        _mediator = mediator;

        RuleFor(x => x.Id)
            .NotNull();

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.MaxSpeed)
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.MaxFlightTime)
            .NotNull()
            .GreaterThan(TimeSpan.Zero);

        // TODO: Add validation rules for UavModel properties.
    }
}