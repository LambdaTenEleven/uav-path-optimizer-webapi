namespace UavPathOptimization.Domain.Contracts.OptimizePath;

public class UAVPath
{
    public int UAVId { get; set; }

    public IList<GeoCoordinateDto> Path { get; set; } = new List<GeoCoordinateDto>();

    public double Distance { get; set; }
}