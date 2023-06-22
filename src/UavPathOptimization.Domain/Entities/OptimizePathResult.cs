using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Contracts.OptimizePath;

namespace UavPathOptimization.Domain.Entities;

public record OptimizePathResult(
    IList<UAVPath> UavPaths
);