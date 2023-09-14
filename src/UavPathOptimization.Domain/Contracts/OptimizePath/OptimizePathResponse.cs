namespace UavPathOptimization.Domain.Contracts.OptimizePath;

public record OptimizePathResponse(
    IList<UavPath> UavPaths
);