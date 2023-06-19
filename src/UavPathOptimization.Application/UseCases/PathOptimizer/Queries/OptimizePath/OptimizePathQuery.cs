using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.UseCases.PathOptimizer.Queries.OptimizePath;

public record OptimizePathQuery(IList<GeoCoordinateDto> path)
    : IRequest<ErrorOr<OptimizePathResult>>;