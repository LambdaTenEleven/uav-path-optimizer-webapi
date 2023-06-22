namespace UavPathOptimization.Domain.Contracts.OptimizePath;

public class OptimizePathRequest
{
    public int UAVCount { get; set; }

    public IList<GeoCoordinateDto> Coordinates { get; set; } = new List<GeoCoordinateDto>();
}