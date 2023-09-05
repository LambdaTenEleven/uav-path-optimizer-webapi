using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Services;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities.Results;
using UavPathOptimization.Domain.Entities.UavEntities;
using UavPathOptimization.Domain.Repositories;

namespace UavPathOptimization.Application.UseCases.Schedule.Queries;
// TODO: move business logic to domain layer
internal sealed class CreateScheduleQueryHandler : IRequestHandler<CreateScheduleQuery, ErrorOr<UavScheduleResult>>
{
    private readonly IDistanceCalculator _distanceCalculator;
    private readonly IUavModelRepository _uavModelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateScheduleQueryHandler(IDistanceCalculator distanceCalculator, IUavModelRepository uavModelRepository, IUnitOfWork unitOfWork)
    {
        _distanceCalculator = distanceCalculator;
        _uavModelRepository = uavModelRepository;
        _unitOfWork = unitOfWork;
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
        var uav = await _uavModelRepository.GetByIdAsync(path.UavModelId, cancellationToken);

        if (uav is null)
        {
            return Errors.UavModelErrors.UavModelNotFound;
        }

        var scheduleUav = new List<UavScheduleEntry>();

        // calculate first point
        var start = new UavScheduleEntry
        {
            Location = path.Coordinates[0],
            IsPBR = false,
            ArrivalTime = null,
            DepartureTime = departureTimeStart,
            TimeSpent = TimeSpan.Zero,
            BatteryTimeLeft = uav.MaxFlightTime
        };

        scheduleUav.Add(start);

        for (int i = 1; i < path.Coordinates.Count; i++)
        {
            var distanceMeters = _distanceCalculator.CalculateDistance(path.Coordinates[i], path.Coordinates[i - 1]);
            var flightTime = (distanceMeters / uav.MaxSpeed).ToTimeSpan();

            if (flightTime + monitoringTime > uav.MaxFlightTime)
            {
                return Errors.Schedule.UavModelMaxFlightTimeExceeded;
            }

            DateTime arrivalTime;
            if (scheduleUav[i - 1].ArrivalTime is null)
            {
                arrivalTime = (DateTime)(scheduleUav[i - 1].DepartureTime! + flightTime + scheduleUav[i - 1].TimeSpent);
            }
            else
            {
                arrivalTime = (DateTime)(scheduleUav[i - 1].ArrivalTime! + flightTime + scheduleUav[i - 1].TimeSpent);
            }

            var isPBR = false;
            var timeLeft = scheduleUav[i - 1].BatteryTimeLeft - monitoringTime - flightTime;

            if (timeLeft <= TimeSpan.Zero)
            {
                timeLeft = uav.MaxFlightTime;
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

            scheduleUav.Add(entry);
        }

        // calculate last point
        var distanceToDH = _distanceCalculator.CalculateDistance(path.Coordinates[0], scheduleUav.Last().Location);
        var flightTimeToDH = (distanceToDH / uav.MaxSpeed).ToTimeSpan();

        if (flightTimeToDH + monitoringTime > uav.MaxFlightTime)
        {
            return Errors.Schedule.UavModelMaxFlightTimeExceeded;
        }

        if (flightTimeToDH + monitoringTime > scheduleUav.Last().BatteryTimeLeft)
        {
            var lastEntry = scheduleUav.Last();
            lastEntry.TimeSpent += chargingTime;
            lastEntry.DepartureTime += chargingTime;
            lastEntry.IsPBR = true;
            lastEntry.BatteryTimeLeft = uav.MaxFlightTime;
        }

        var arrival = scheduleUav.Last().DepartureTime + flightTimeToDH;

        var timeLeftLast = scheduleUav.Last().BatteryTimeLeft - monitoringTime - flightTimeToDH;
        var endPoint = new UavScheduleEntry
        {
            Location = path.Coordinates[0],
            IsPBR = false,
            ArrivalTime = arrival,
            DepartureTime = null,
            TimeSpent = TimeSpan.Zero,
            BatteryTimeLeft = timeLeftLast
        };

        scheduleUav.Add(endPoint);

        return new UavPathSchedule
        {
            UavModelId = path.UavModelId,
            UavScheduleEntries = scheduleUav
        };
    }
}