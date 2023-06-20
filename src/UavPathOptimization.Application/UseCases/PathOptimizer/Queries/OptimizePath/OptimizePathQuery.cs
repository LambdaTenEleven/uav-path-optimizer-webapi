using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.UseCases.PathOptimizer.Queries.OptimizePath;

public record OptimizePathQuery(IList<GeoCoordinateDto> Path)
    : IRequest<ErrorOr<OptimizePathResult>>;