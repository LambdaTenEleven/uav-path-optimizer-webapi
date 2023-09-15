namespace UavPathOptimization.Domain.Contracts.OptimizePath;

public class UavPath
{
    public int VehicleId { get; set; }

    public IList<GeoCoordinateDto> Path { get; set; } = new List<GeoCoordinateDto>();

    public double Distance { get; set; }
}