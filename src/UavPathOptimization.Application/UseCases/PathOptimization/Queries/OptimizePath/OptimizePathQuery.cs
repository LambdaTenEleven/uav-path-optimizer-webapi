using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities.Results;

namespace UavPathOptimization.Application.UseCases.PathOptimization.Queries.OptimizePath;

public sealed record OptimizePathQuery(
    int UavCount,
    IList<GeoCoordinateDto> Coordinates
) : IRequest<ErrorOr<OptimizePathResult>>;