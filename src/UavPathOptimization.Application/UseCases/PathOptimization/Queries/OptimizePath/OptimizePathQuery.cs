using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities.Results;

namespace UavPathOptimization.Application.UseCases.PathOptimization.Queries.OptimizePath;

public record OptimizePathQuery(
    int UAVCount,
    IList<GeoCoordinateDto> Coordinates
) : IRequest<ErrorOr<OptimizePathResult>>;