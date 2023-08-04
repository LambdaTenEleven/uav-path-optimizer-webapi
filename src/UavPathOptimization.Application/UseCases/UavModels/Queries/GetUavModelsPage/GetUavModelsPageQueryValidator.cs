using FluentValidation;
using UavPathOptimization.Domain.Common.ValidationRules;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModelsPage;

public sealed class GetUavModelsPageQueryValidator : AbstractValidator<GetUavModelsPageQuery>
{
    public GetUavModelsPageQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be greater than or equal to 1.");

        RuleFor(x => x.SortField)
            .Must(PropertyNameValidationRules.MustBeValidPropertyName<UavModel>)
            .WithMessage("Sort field must be a valid property name of UavModel.");
    }
}