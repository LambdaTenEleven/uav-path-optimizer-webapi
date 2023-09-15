using System.Diagnostics;
using UavPathOptimization.Domain.Contracts;

namespace UavPathOptimization.Domain.Entities.Schedule;

/// <summary>
/// Schedule entry is a location with arrival and departure time and time spent at the location
/// </summary>
[DebuggerDisplay("ARR: {ArrivalTime}, DEP: {DepartureTime}, TIME: {TimeSpent}")]
public class ScheduleEntry
{
    public ScheduleEntry(GeoCoordinateDto Location, DateTime? ArrivalTime, DateTime? DepartureTime,
        TimeSpan TimeSpent)
    {
        this.Location = Location;
        this.ArrivalTime = ArrivalTime;
        this.DepartureTime = DepartureTime;
        this.TimeSpent = TimeSpent;
    }

    public GeoCoordinateDto Location { get; set; }
    public DateTime? ArrivalTime { get; set; }
    public DateTime? DepartureTime { get; set; }
    public TimeSpan TimeSpent { get; set; }
}
