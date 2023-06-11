using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Contracts.OptimizePath;

namespace UavPathOptimization.Application.UseCases.PathOptimizer.Queries;

public record OptimizePathQuery(IList<GeoCoordinateDto> path)
    : IRequest<ErrorOr<OptimizePathResponse>>;