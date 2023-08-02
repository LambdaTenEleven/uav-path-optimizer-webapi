using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Persistence.Uav;
using UavPathOptimization.Application.Common.Services;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities.Results;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.Schedule.Queries;

public class CreateScheduleHandler : IRequestHandler<CreateScheduleQuery, ErrorOr<UavScheduleResult>>
{
    private readonly IMediator _mediator;

    private readonly IDistanceCalculator _distanceCalculator;

    public CreateScheduleHandler(IMediator mediator, IDistanceCalculator distanceCalculator)
    {
        _mediator = mediator;
        _distanceCalculator = distanceCalculator;
    }

    public async Task<ErrorOr<UavScheduleResult>> Handle(CreateScheduleQuery request, CancellationToken cancellationToken)
    {
        // UAV schedules
        var schedules = new List<UavPathSchedule>();

        foreach (var path in request.Paths)
        {
            var schedulePathResult = await CalculateScheduleForUavPath(path, request.DepartureTimeStart, request.MonitoringTime, request.ChargingTime, cancellationToken);
            if (schedulePathResult.IsError)
            {
                return schedulePathResult.Errors;
            }

            schedules.Add(schedulePathResult.Value);
        }

        // TODO ABRAS schedules

        return new UavScheduleResult(schedules, new List<UavScheduleEntry>());
    }

    private async Task<ErrorOr<UavPathSchedule>> CalculateScheduleForUavPath(UavPathDto path, DateTime departureTimeStart, TimeSpan monitoringTime, TimeSpan chargingTime, CancellationToken cancellationToken)
    {
        var uav = await _mediator.Send(new GetUavModelFromDbByIdQuery(path.UavModelId), cancellationToken);
        if (uav.FirstError.Type == ErrorType.NotFound)
        {
            return Errors.UavModelErrors.UavModelNotFound;
            //TODO complete error handling
        }

        var scheduleUAV = new List<UavScheduleEntry>();

        // calculate first point
        var start = new UavScheduleEntry
        {
            Location = path.Coordinates[0],
            IsPBR = false,
            ArrivalTime = null,
            DepartureTime = departureTimeStart,
            TimeSpent = TimeSpan.Zero,
            BatteryTimeLeft = uav.Value.MaxFlightTime
        };

        scheduleUAV.Add(start);

        for (int i = 1; i < path.Coordinates.Count; i++)
        {
            var distanceMeters = _distanceCalculator.CalculateDistance(path.Coordinates[i], path.Coordinates[i - 1]);
            var flightTime = (distanceMeters / uav.Value.MaxSpeed).ToTimeSpan();

            if (flightTime + monitoringTime > uav.Value.MaxFlightTime)
            {
                return Errors.Schedule.UavModelMaxFlightTimeExceeded;
            }

            DateTime arrivalTime;
            if (scheduleUAV[i - 1].ArrivalTime is null)
            {
                arrivalTime = (DateTime)(scheduleUAV[i - 1].DepartureTime! + flightTime + scheduleUAV[i - 1].TimeSpent);
            }
            else
            {
                arrivalTime = (DateTime)(scheduleUAV[i - 1].ArrivalTime! + flightTime + scheduleUAV[i - 1].TimeSpent);
            }

            var isPBR = false;
            var timeLeft = scheduleUAV[i - 1].BatteryTimeLeft - monitoringTime - flightTime;

            if (timeLeft <= TimeSpan.Zero)
            {
                timeLeft = uav.Value.MaxFlightTime;
                isPBR = true;
            }

            var timeSpent = isPBR ? monitoringTime + chargingTime : monitoringTime;
            var departureTime = arrivalTime + timeSpent;

            var entry = new UavScheduleEntry
            {
                Location = path.Coordinates[i],
                IsPBR = isPBR,
                ArrivalTime = arrivalTime,
                DepartureTime = departureTime,
                TimeSpent = timeSpent,
                BatteryTimeLeft = timeLeft
            };

            scheduleUAV.Add(entry);
        }

        // calculate last point
        var distanceToDH = _distanceCalculator.CalculateDistance(path.Coordinates[0], scheduleUAV.Last().Location);
        var flightTimeToDH = (distanceToDH / uav.Value.MaxSpeed).ToTimeSpan();

        if (flightTimeToDH + monitoringTime > uav.Value.MaxFlightTime)
        {
            return Errors.Schedule.UavModelMaxFlightTimeExceeded;
        }

        if (flightTimeToDH + monitoringTime > scheduleUAV.Last().BatteryTimeLeft)
        {
            var lastEntry = scheduleUAV.Last();
            lastEntry.TimeSpent += chargingTime;
            lastEntry.DepartureTime += chargingTime;
            lastEntry.IsPBR = true;
            lastEntry.BatteryTimeLeft = uav.Value.MaxFlightTime;
        }

        var arrival = scheduleUAV.Last().DepartureTime + flightTimeToDH;

        var timeLeftLast = scheduleUAV.Last().BatteryTimeLeft - monitoringTime - flightTimeToDH;
        var endPoint = new UavScheduleEntry
        {
            Location = path.Coordinates[0],
            IsPBR = false,
            ArrivalTime = arrival,
            DepartureTime = null,
            TimeSpent = TimeSpan.Zero,
            BatteryTimeLeft = timeLeftLast
        };

        scheduleUAV.Add(endPoint);

        return new UavPathSchedule
        {
            UavModelId = path.UavModelId,
            UavScheduleEntries = scheduleUAV
        };
    }
}