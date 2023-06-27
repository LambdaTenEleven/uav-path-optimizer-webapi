using ErrorOr;
using MediatR;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.CreateUavModel;

public record CreateUavModelCommand(
    string Name,
    double MaxSpeed,
    TimeSpan MaxFlightTime
) : IRequest<ErrorOr<Guid>>;