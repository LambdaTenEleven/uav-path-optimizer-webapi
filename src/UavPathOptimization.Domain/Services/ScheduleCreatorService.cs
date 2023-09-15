using ErrorOr;
using GeoCoordinatePortable;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities.Schedule;
using UavPathOptimization.Domain.Entities.UavEntities;
using UnitsNet;
using UnitsNet.Units;

namespace UavPathOptimization.Domain.Services;

public class ScheduleCreatorService : IScheduleCreatorService
{
    public ErrorOr<UavSchedule> CreateScheduleForUavPath(UavPathDto path, DateTime departureTimeStart, TimeSpan monitoringTime,
        TimeSpan chargingTime, UavModel uavModel)
    {
        var scheduleUav = new List<UavScheduleEntry>();

        // calculate first point
        var start = new UavScheduleEntry(path.Coordinates[0], null, departureTimeStart, TimeSpan.Zero, false, uavModel.MaxFlightTime);

        scheduleUav.Add(start);

        for (int i = 1; i < path.Coordinates.Count; i++)
        {
            var distanceMeters = CalculateDistance(path.Coordinates[i], path.Coordinates[i - 1]);
            var flightTime = (distanceMeters / uavModel.MaxSpeed).ToTimeSpan();

            if (flightTime + monitoringTime > uavModel.MaxFlightTime)
            {
                return Errors.Schedule.UavModelMaxFlightTimeExceeded;
            }

            var arrivalTime = (scheduleUav[i - 1].ArrivalTime ?? scheduleUav[i - 1].DepartureTime) + flightTime + scheduleUav[i - 1].TimeSpent;

            var isPBR = false;
            var timeLeft = scheduleUav[i - 1].BatteryTimeLeft - monitoringTime - flightTime;

            if (timeLeft <= TimeSpan.Zero)
            {
                timeLeft = uavModel.MaxFlightTime;
                isPBR = true;
            }

            var timeSpent = isPBR ? monitoringTime + chargingTime : monitoringTime;
            var departureTime = arrivalTime + timeSpent;

            var entry = new UavScheduleEntry(path.Coordinates[i], arrivalTime, departureTime, timeSpent, isPBR, timeLeft);

            scheduleUav.Add(entry);
        }

        // calculate last point
        var distanceToDH = CalculateDistance(path.Coordinates[0], scheduleUav.Last().Location);
        var flightTimeToDH = (distanceToDH / uavModel.MaxSpeed).ToTimeSpan();

        if (flightTimeToDH + monitoringTime > uavModel.MaxFlightTime)
        {
            return Errors.Schedule.UavModelMaxFlightTimeExceeded;
        }

        if (flightTimeToDH + monitoringTime > scheduleUav.Last().BatteryTimeLeft)
        {
            var lastEntry = scheduleUav.Last();
            lastEntry.TimeSpent += chargingTime;
            lastEntry.DepartureTime += chargingTime;
            lastEntry.IsPBR = true;
            lastEntry.BatteryTimeLeft = uavModel.MaxFlightTime;
        }

        var arrival = scheduleUav.Last().DepartureTime + flightTimeToDH;

        var timeLeftLast = scheduleUav.Last().BatteryTimeLeft - monitoringTime - flightTimeToDH;

        var endPoint = new UavScheduleEntry(path.Coordinates[0], arrival, null, TimeSpan.Zero, false, timeLeftLast);

        scheduleUav.Add(endPoint);

        return new UavSchedule(path.UavModelId, scheduleUav);
    }

    public ErrorOr<AbrasSchedule> CreateScheduleForAbrasPath(IList<GeoCoordinateDto> optimizedPath, DateTime departureTimeStart, TimeSpan operatingTime,
        Speed abrasSpeed, GeoCoordinateDto abrasStartPoint)
    {
        // start calculating schedule entries for Abras
        var scheduleAbras = new List<ScheduleEntry>();

        // calculate first point
        var start = new ScheduleEntry(optimizedPath[0], null, departureTimeStart, TimeSpan.Zero);

        scheduleAbras.Add(start);

        for (int i = 1; i < optimizedPath.Count; i++)
        {
            var distanceMeters = CalculateDistance(optimizedPath[i], optimizedPath[i - 1]);
            var flightTime = (distanceMeters / abrasSpeed).ToTimeSpan();

            var arrivalTime = (scheduleAbras[i - 1].ArrivalTime ?? scheduleAbras[i - 1].DepartureTime) + flightTime + scheduleAbras[i - 1].TimeSpent;

            var timeSpent = operatingTime;
            var departureTime = arrivalTime + timeSpent;

            var entry = new ScheduleEntry(optimizedPath[i], arrivalTime, departureTime, timeSpent);

            scheduleAbras.Add(entry);
        }

        // calculate last point
        var distanceToDH = CalculateDistance(optimizedPath[0], scheduleAbras.Last().Location);
        var flightTimeToDH = (distanceToDH / abrasSpeed).ToTimeSpan();

        var arrival = scheduleAbras.Last().DepartureTime + flightTimeToDH;

        var endPoint = new ScheduleEntry(optimizedPath[0], arrival, null, TimeSpan.Zero);

        scheduleAbras.Add(endPoint);

        return new AbrasSchedule(scheduleAbras);
    }

    private static Length CalculateDistance(GeoCoordinateDto point1, GeoCoordinateDto point2)
    {
        var p1 = new GeoCoordinate(point1.Latitude, point1.Longitude);
        var p2 = new GeoCoordinate(point2.Latitude, point2.Longitude);

        return new Length(p1.GetDistanceTo(p2), LengthUnit.Meter);
    }
}