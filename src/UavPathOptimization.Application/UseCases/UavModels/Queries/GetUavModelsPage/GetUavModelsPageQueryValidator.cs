using FluentValidation;

namespace UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModelsPage;

public class GetUavModelsPageQueryValidator : AbstractValidator<GetUavModelsPageQuery>
{
    public GetUavModelsPageQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be greater than or equal to 1.");
    }
}