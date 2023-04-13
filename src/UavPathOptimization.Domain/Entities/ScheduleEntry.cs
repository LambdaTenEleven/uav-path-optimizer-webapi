using GeoCoordinatePortable;

namespace UavPathOptimization.Domain.Entities;

public class ScheduleEntry
{
    public GeoCoordinate GeoPoint { get; set; }

    public DateTime Arrival { get; set; }

    public DateTime Departure { get; set; }

    public TimeSpan TimeSpent { get; set; }

    public TimeSpan BatteryLifetimeLeft { get; set; }
}