using FluentValidation;

namespace UavPathOptimization.Application.UseCases.PathOptimization.Queries.OptimizePath;

public class OptimizePathQueryValidator : AbstractValidator<OptimizePathQuery>
{
    public OptimizePathQueryValidator()
    {
        RuleFor(x => x.Coordinates)
            .NotEmpty();

        RuleFor(x => x.Coordinates.Count)
            .GreaterThan(2)
            .GreaterThan(x => x.UAVCount);

        RuleForEach(x => x.Coordinates)
            .ChildRules(x =>
            {
                x.RuleFor(x => x.Latitude)
                    .InclusiveBetween(-90, 90);

                x.RuleFor(x => x.Longitude)
                    .InclusiveBetween(-180, 180);
            });

        RuleFor(x => x.UAVCount)
            .GreaterThan(0);
    }
}