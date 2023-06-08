using ErrorOr;
using GeoCoordinatePortable;
using MediatR;

namespace UavPathOptimization.Application.UseCases.PathOptimizer.Queries;

public record OptimizePathQuery(IList<GeoCoordinate> path)
    : IRequest<ErrorOr<IList<GeoCoordinate>>>;