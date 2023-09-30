using ErrorOr;
using GeoCoordinatePortable;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities.Results;
using UavPathOptimization.Domain.Entities.Schedule;

namespace UavPathOptimization.Application.Common.Services;

public class AbrasScheduleCreator : IAbrasScheduleCreator
{
    public ErrorOr<UavScheduleResult> CreateScheduleForAbras(IList<UavSchedule> uavSchedules,
        double speed,
        GeoCoordinateDto abrasDepotLocation)
    {
        var pbrEntries = GetPbrEntries(uavSchedules);
        var currentLocation = abrasDepotLocation;
        var currentTime = DateTime.MinValue;

        var abrasScheduleEntries = new List<ScheduleEntry>();

        foreach (var (uavSchedule, pbrEntry) in pbrEntries)
        {
            var elapsedTime = CalculateElapsedTime(currentLocation, pbrEntry.Location, speed);

            if (pbrEntry.ArrivalTime - elapsedTime <= currentTime.AddMinutes(1))
            {
                // Conflict: Shift the UAV's schedule
                var conflictDuration = currentTime.AddMinutes(1).Add(elapsedTime) - pbrEntry.ArrivalTime!.Value;
                ShiftUavSchedule(uavSchedule, conflictDuration);
            }

            currentTime = pbrEntry.ArrivalTime.Value.AddMinutes(-1);
            var departureTime = pbrEntry.ArrivalTime.Value.Add(pbrEntry.TimeSpent);
            abrasScheduleEntries.Add(new ScheduleEntry(pbrEntry.Location, currentTime, departureTime,
                departureTime - currentTime));
            currentLocation = pbrEntry.Location;
        }

        return new UavScheduleResult(uavSchedules, new AbrasSchedule(abrasScheduleEntries));
    }

    private static List<Tuple<UavSchedule, UavScheduleEntry>> GetPbrEntries(IList<UavSchedule> uavSchedules)
    {
        var pbrEntries = new List<Tuple<UavSchedule, UavScheduleEntry>>();

        foreach (var uavSchedule in uavSchedules)
        {
            pbrEntries.AddRange(uavSchedule.UavScheduleEntries.Where(entry => entry.IsPBR)
                .Select(entry => Tuple.Create(uavSchedule, entry)));
        }

        pbrEntries.Sort((a, b) => a.Item2.ArrivalTime.Value.CompareTo(b.Item2.ArrivalTime.Value));

        return pbrEntries;
    }

    private static TimeSpan CalculateElapsedTime(GeoCoordinateDto start, GeoCoordinateDto end, double speed)
    {
        var startCoord = new GeoCoordinate(start.Latitude, start.Longitude);
        var endCoord = new GeoCoordinate(end.Latitude, end.Longitude);

        var distance = startCoord.GetDistanceTo(endCoord);

        return TimeSpan.FromHours(distance / (speed * 1000)); // Assuming speed is in km/h
    }

    private static void ShiftUavSchedule(UavSchedule uavSchedule, TimeSpan shiftDuration)
    {
        foreach (var entry in uavSchedule.UavScheduleEntries)
        {
            entry.ArrivalTime = entry.ArrivalTime?.Add(shiftDuration);
            entry.DepartureTime = entry.DepartureTime?.Add(shiftDuration);
        }
    }
}