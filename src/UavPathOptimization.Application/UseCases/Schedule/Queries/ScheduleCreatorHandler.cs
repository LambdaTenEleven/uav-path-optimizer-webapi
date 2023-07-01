using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Services;
using UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModel;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Contracts;
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
        var uav = await _mediator.Send(new GetUavModelQuery(request.UavId), cancellationToken);
        if (uav.FirstError.Type == ErrorType.NotFound)
        {
            return Errors.UavModelErrors.UavModelNotFound;
            //TODO complete error handling
        }

        var scheduleUAV = new List<UavScheduleEntry>();

        var start = new UavScheduleEntry()
        {
            Location = request.Path[0],
            IsPBR = false,
            ArrivalTime = request.DepartureTimeStart,
            DepartureTime = request.DepartureTimeStart,
            TimeSpent = TimeSpan.Zero,
            BatteryTimeLeft = uav.Value.MaxFlightTime
        };

        scheduleUAV.Add(start);

        for (int i = 1; i < request.Path.Count; i++)
        {
            var flightTime = CalculateFlightTime(request.Path[i], request.Path[i - 1]);
            var arrivalTime = scheduleUAV[i - 1].ArrivalTime + flightTime + scheduleUAV[i - 1].TimeSpent;
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

        var flightTimeToDH = CalculateFlightTime(request.Path[0], scheduleUAV.Last().Location);
        var arrival = scheduleUAV.Last().DepartureTime + flightTimeToDH;

        var endPoint = new UavScheduleEntry
        {
            Location = request.Path[0],
            IsPBR = false,
            ArrivalTime = arrival,
            DepartureTime = DateTime.MaxValue,
            TimeSpent = TimeSpan.Zero,
            BatteryTimeLeft = scheduleUAV.Last().BatteryTimeLeft
        };

        scheduleUAV.Add(endPoint);

        return new UavScheduleResult
        {
            UavModelId = request.UavId,
            UavScheduleEntries = scheduleUAV
        };
    }

    private TimeSpan CalculateFlightTime(GeoCoordinateDto point1, GeoCoordinateDto point2)
    {
        var distance = _distanceCalculator.CalculateDistance(point1, point2);

        // caclulate flight time in m/s

        //TODO
        return TimeSpan.Zero;
    }
}