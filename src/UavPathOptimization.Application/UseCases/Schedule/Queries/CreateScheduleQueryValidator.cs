using FluentValidation;

namespace UavPathOptimization.Application.UseCases.Schedule.Queries;

public sealed class CreateScheduleQueryValidator : AbstractValidator<CreateScheduleQuery>
{
    public CreateScheduleQueryValidator()
    {
        RuleFor(x => x.Paths)
            .NotEmpty();

        RuleForEach(x => x.Paths)
            .ChildRules(x =>
            {
                x.RuleFor(x => x.Coordinates)
                    .NotEmpty();

                x.RuleFor(x => x.Coordinates.Count)
                    .GreaterThan(2);

                x.RuleForEach(x => x.Coordinates)
                    .ChildRules(x =>
                    {
                        x.RuleFor(x => x.Latitude)
                            .InclusiveBetween(-90, 90);

                        x.RuleFor(x => x.Longitude)
                            .InclusiveBetween(-180, 180);
                    });

                x.RuleFor(x => x.UavModelId)
                    .NotEmpty();
            });

        RuleFor(x => x.DepartureTimeStart)
            .NotEmpty();

        RuleFor(x => x.MonitoringTime)
            .NotEmpty();

        RuleFor(x => x.ChargingTime)
            .NotEmpty();

        RuleFor(x => x.AbrasSpeed)
            .GreaterThan(0);

        RuleFor(x => x.AbrasDepotLocation)
            .NotNull();
    }
}