using ErrorOr;
using GeoCoordinatePortable;
using MediatR;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Contracts.OptimizePath;
using UavPathOptimization.Domain.Entities.Results;
using UavPathOptimization.Domain.Services;

namespace UavPathOptimization.Application.UseCases.PathOptimization.Queries.OptimizePath;

internal sealed class OptimizePathQueryHandler : IRequestHandler<OptimizePathQuery, ErrorOr<OptimizePathResult>>
{
    private readonly IPathOptimizationService pathOptimizationService;

    public OptimizePathQueryHandler(IPathOptimizationService pathOptimizationService)
    {
        this.pathOptimizationService = pathOptimizationService;
    }

    public Task<ErrorOr<OptimizePathResult>> Handle(OptimizePathQuery request, CancellationToken cancellationToken)
    {
        var result = pathOptimizationService.OptimizePath(request.UavCount, request.Coordinates);

        if (result.IsError)
        {
            return Task.FromResult<ErrorOr<OptimizePathResult>>(result.FirstError);
        }

        return Task.FromResult<ErrorOr<OptimizePathResult>>(new OptimizePathResult(result.Value));
    }
}