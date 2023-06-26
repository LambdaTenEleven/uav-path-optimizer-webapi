using ErrorOr;
using MediatR;

namespace UavPathOptimization.Application.UseCases.Uav.Commands.CreateUav;

public record CreateUavModelCommand(
    string Name,
    double MaxSpeed,
    TimeSpan MaxFlightTime
) : IRequest<ErrorOr<Unit>>;