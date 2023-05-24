using FluentResults;
using GeoCoordinatePortable;
using MediatR;

namespace UavPathOptimization.Application.UseCases.PathOptimizer.Queries;

public record OptimizePathQuery(IList<GeoCoordinate> path)
    : IRequest<Result<IList<GeoCoordinate>>>;