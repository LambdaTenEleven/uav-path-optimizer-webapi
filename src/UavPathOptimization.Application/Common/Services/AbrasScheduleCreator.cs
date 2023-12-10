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
        GeoCoordinateDto abrasDepotLocation,
        DateTime departureTimeStart,
        TimeSpan chargingTime)
    {
        var pbrEntries = GetPbrEntries(uavSchedules);

        if(pbrEntries.Count == 0)
        {
            return new UavScheduleResult(uavSchedules, new AbrasSchedule(new List<ScheduleEntry>()));
        }

        var currentLocation = abrasDepotLocation;
        var currentTime = DateTime.MinValue;

        var abrasScheduleEntries = new List<ScheduleEntry>
        {
            // Add depot entry
            new ScheduleEntry(abrasDepotLocation, null, departureTimeStart, TimeSpan.Zero)
        };

        foreach (var (uavSchedule, pbrEntry) in pbrEntries)
        {
            var elapsedTime = CalculateElapsedTime(currentLocation, pbrEntry.Location, speed);

            // Wait time for ABRAS to reach the PBR point before the UAV arrives
            var waitTime = TimeSpan.Zero;
            if (pbrEntry.ArrivalTime - elapsedTime > currentTime)
            {
                waitTime = pbrEntry.ArrivalTime.Value - elapsedTime - currentTime;
            }
            else
            {
                // Conflict: Shift the UAV's schedule
                var conflictDuration = currentTime.Add(elapsedTime) - pbrEntry.ArrivalTime.Value.AddMinutes(-1);
                ShiftUavSchedule(uavSchedule, conflictDuration);
            }

            currentTime += elapsedTime + waitTime;
            var departureTime = pbrEntry.ArrivalTime.Value.Add(pbrEntry.TimeSpent);
            abrasScheduleEntries.Add(new ScheduleEntry(pbrEntry.Location, currentTime, departureTime, elapsedTime));

            currentTime = departureTime;
            currentLocation = pbrEntry.Location;
        }

        // calculate the time to return to the depot
        var returnTime = CalculateElapsedTime(currentLocation, abrasDepotLocation, speed);

        // Add depot entry
        abrasScheduleEntries.Add(
            new ScheduleEntry(abrasDepotLocation, currentTime.Add(returnTime), null, TimeSpan.Zero));

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