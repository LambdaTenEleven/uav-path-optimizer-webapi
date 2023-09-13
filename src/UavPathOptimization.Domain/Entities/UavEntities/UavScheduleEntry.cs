using UavPathOptimization.Domain.Contracts;

namespace UavPathOptimization.Domain.Entities.UavEntities;

/// <summary>
/// Uav schedule entry is a schedule entry with additional information about the UAV (e.g. battery time left)
/// </summary>
/// <seealso cref="ScheduleEntry" />
public class UavScheduleEntry : ScheduleEntry
{
    public UavScheduleEntry(GeoCoordinateDto Location,
        DateTime? ArrivalTime,
        DateTime? DepartureTime,
        TimeSpan TimeSpent,
        bool IsPBR,
        TimeSpan BatteryTimeLeft) : base(Location,
        ArrivalTime,
        DepartureTime,
        TimeSpent)
    {
        this.IsPBR = IsPBR;
        this.BatteryTimeLeft = BatteryTimeLeft;
    }

    public bool IsPBR { get; set; }
    public TimeSpan BatteryTimeLeft { get; set; }
}