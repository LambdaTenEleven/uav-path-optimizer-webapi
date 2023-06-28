using FluentValidation;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;

public class UpdateUavModelCommandValidator : AbstractValidator<UpdateUavModelCommand>
{
    private readonly IMediator _mediator;

    public UpdateUavModelCommandValidator(IMediator mediator)
    {
        _mediator = mediator;

        RuleFor(x => x.UavModel)
            .NotNull();

        // TODO: Add validation rules for UavModel properties.
    }
}