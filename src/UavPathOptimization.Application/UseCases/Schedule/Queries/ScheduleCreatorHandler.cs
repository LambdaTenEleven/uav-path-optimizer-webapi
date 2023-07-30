using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Services;
using UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModel;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities.Results;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.Schedule.Queries;

public class ScheduleCreatorHandler : IRequestHandler<ScheduleCreatorQuery, ErrorOr<UavScheduleResult>>
{
    private readonly IMediator _mediator;

    private readonly IDistanceCalculator _distanceCalculator;

    public ScheduleCreatorHandler(IMediator mediator, IDistanceCalculator distanceCalculator)
    {
        _mediator = mediator;
        _distanceCalculator = distanceCalculator;
    }

    public async Task<ErrorOr<UavScheduleResult>> Handle(ScheduleCreatorQuery request, CancellationToken cancellationToken)
    {
        var uav = await _mediator.Send(new GetUavModelQuery(request.UavModelId), cancellationToken);
        if (uav.FirstError.Type == ErrorType.NotFound)
        {
            return Errors.UavModelErrors.UavModelNotFound;
            //TODO complete error handling
        }

        var scheduleUAV = new List<UavScheduleEntry>();

        // calculate first point
        var start = new UavScheduleEntry
        {
            Location = request.Path[0],
            IsPBR = false,
            ArrivalTime = null,
            DepartureTime = request.DepartureTimeStart,
            TimeSpent = TimeSpan.Zero,
            BatteryTimeLeft = uav.Value.MaxFlightTime
        };

        scheduleUAV.Add(start);

        for (int i = 1; i < request.Path.Count; i++)
        {
            var distanceMeters = _distanceCalculator.CalculateDistance(request.Path[i], request.Path[i - 1]);
            var flightTime = (distanceMeters / uav.Value.MaxSpeed).ToTimeSpan();

            if (flightTime + request.MonitoringTime > uav.Value.MaxFlightTime)
            {
                return Errors.UavModelErrors.UavModelMaxFlightTimeExceeded;
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
            var timeLeft = scheduleUAV[i - 1].BatteryTimeLeft - request.MonitoringTime - flightTime;

            if (timeLeft <= TimeSpan.Zero)
            {
                timeLeft = uav.Value.MaxFlightTime;
                isPBR = true;
            }

            var timeSpent = isPBR ? request.MonitoringTime + request.ChargingTime : request.MonitoringTime;
            var departureTime = arrivalTime + timeSpent;

            var entry = new UavScheduleEntry
            {
                Location = request.Path[i],
                IsPBR = isPBR,
                ArrivalTime = arrivalTime,
                DepartureTime = departureTime,
                TimeSpent = timeSpent,
                BatteryTimeLeft = timeLeft
            };

            scheduleUAV.Add(entry);
        }

        // calculate last point
        var distanceToDH = _distanceCalculator.CalculateDistance(request.Path[0], scheduleUAV.Last().Location);
        var flightTimeToDH = (distanceToDH / uav.Value.MaxSpeed).ToTimeSpan();

        if (flightTimeToDH + request.MonitoringTime > uav.Value.MaxFlightTime)
        {
            return Errors.UavModelErrors.UavModelMaxFlightTimeExceeded;
        }

        var arrival = scheduleUAV.Last().DepartureTime + flightTimeToDH;

        var timeLeftLast = scheduleUAV.Last().BatteryTimeLeft - request.MonitoringTime - flightTimeToDH;
        var endPoint = new UavScheduleEntry
        {
            Location = request.Path[0],
            IsPBR = false,
            ArrivalTime = arrival,
            DepartureTime = null,
            TimeSpent = TimeSpan.Zero,
            BatteryTimeLeft = timeLeftLast
        };

        scheduleUAV.Add(endPoint);

        return new UavScheduleResult
        {
            UavModelId = request.UavModelId,
            UavScheduleEntries = scheduleUAV
        };
    }
}