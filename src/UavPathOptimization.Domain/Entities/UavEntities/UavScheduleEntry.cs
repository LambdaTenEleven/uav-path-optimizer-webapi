using UavPathOptimization.Domain.Contracts;

namespace UavPathOptimization.Domain.Entities.UavEntities;

public record UavScheduleEntry
{
    public UavScheduleEntry()
    {
    }

    public UavScheduleEntry(GeoCoordinateDto location, bool isPbr, DateTime arrivalTime, DateTime departureTime, TimeSpan timeSpent, TimeSpan batteryTimeLeft)
    {
        Location = location;
        IsPBR = isPbr;
        ArrivalTime = arrivalTime;
        DepartureTime = departureTime;
        TimeSpent = timeSpent;
        BatteryTimeLeft = batteryTimeLeft;
    }

    public GeoCoordinateDto Location { get; set; } = null!;

    public bool IsPBR { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public DateTime? DepartureTime { get; set; }

    public TimeSpan TimeSpent { get; set; }

    public TimeSpan BatteryTimeLeft { get; set; }
}