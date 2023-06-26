using UavPathOptimization.Domain.Contracts.OptimizePath;

namespace UavPathOptimization.Domain.Entities.Results;

public record OptimizePathResult(
    IList<UAVPath> UavPaths
);