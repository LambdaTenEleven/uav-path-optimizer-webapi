using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;

public record UpdateUavModelCommand(
    Guid Id,
    string Name,
    double MaxSpeed,
    TimeSpan MaxFlightTime
) : IRequest<ErrorOr<Unit>>;