namespace UavPathOptimization.Domain.Contracts.OptimizePath;

public record OptimizePathResponse(
    IList<UAVPath> UavPaths
);