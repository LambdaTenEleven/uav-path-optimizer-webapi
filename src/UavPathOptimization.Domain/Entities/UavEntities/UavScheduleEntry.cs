using UavPathOptimization.Domain.Contracts;

namespace UavPathOptimization.Domain.Entities.UavEntities;

public class UavScheduleEntry
{
    public GeoCoordinateDto Location { get; set; } = null!;

    public bool IsPBR { get; set; }

    public DateTime ArrivalTime { get; set; }

    public DateTime DepartureTime { get; set; }

    public TimeSpan TimeSpent { get; set; }

    public TimeSpan BatteryTimeLeft { get; set; }
}