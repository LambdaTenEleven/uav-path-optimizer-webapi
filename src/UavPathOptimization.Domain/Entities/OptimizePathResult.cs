using UavPathOptimization.Domain.Contracts;

namespace UavPathOptimization.Domain.Entities;

public record OptimizePathResult(IList<GeoCoordinateDto> Path, double Distance);