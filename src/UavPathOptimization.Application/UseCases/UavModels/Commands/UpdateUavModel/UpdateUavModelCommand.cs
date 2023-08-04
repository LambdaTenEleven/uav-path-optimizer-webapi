using ErrorOr;
using MediatR;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;

public sealed record UpdateUavModelCommand(
    Guid Id,
    string Name,
    double MaxSpeed,
    TimeSpan MaxFlightTime
) : IRequest<ErrorOr<Unit>>;