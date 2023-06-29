using FluentValidation;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.DeleteUavModel;

public class DeleteUavModelCommandValidator : AbstractValidator<DeleteUavModelCommand>
{
    public DeleteUavModelCommandValidator()
    {
        RuleFor(x => x.id)
            .NotNull()
            .NotEmpty();
    }
}