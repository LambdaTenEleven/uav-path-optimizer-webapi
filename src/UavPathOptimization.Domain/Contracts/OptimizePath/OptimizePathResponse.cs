namespace UavPathOptimization.Domain.Contracts.OptimizePath;

public record OptimizePathResponse(
    IList<GeoCoordinateDto> Path,
    double Distance
);