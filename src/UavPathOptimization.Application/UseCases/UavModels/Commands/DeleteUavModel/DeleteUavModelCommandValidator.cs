using FluentValidation;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.DeleteUavModel;

public sealed class DeleteUavModelCommandValidator : AbstractValidator<DeleteUavModelCommand>
{
    public DeleteUavModelCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty();
    }
}