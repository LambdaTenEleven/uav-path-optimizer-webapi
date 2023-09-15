using System.Diagnostics;
using UavPathOptimization.Domain.Contracts;

namespace UavPathOptimization.Domain.Entities.Schedule;

/// <summary>
/// Uav schedule entry is a schedule entry with additional information about the UAV (e.g. battery time left)
/// </summary>
/// <seealso cref="ScheduleEntry" />
[DebuggerDisplay("ARR: {ArrivalTime}, DEP: {DepartureTime}, TIME: {TimeSpent}, PBR: {IsPBR}, BTL: {BatteryTimeLeft}")]
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