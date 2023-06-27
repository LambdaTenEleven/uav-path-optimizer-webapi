using ErrorOr;
using MediatR;

namespace UavPathOptimization.Application.UseCases.UavModel.Commands.CreateUavModel;

public record CreateUavModelCommand(
    string Name,
    double MaxSpeed,
    TimeSpan MaxFlightTime
) : IRequest<ErrorOr<Unit>>;