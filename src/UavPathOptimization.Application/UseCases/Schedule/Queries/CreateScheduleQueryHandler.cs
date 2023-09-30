using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using UavPathOptimization.Application.Common.Services;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities.Results;
using UavPathOptimization.Domain.Entities.Schedule;
using UavPathOptimization.Domain.Repositories;
using UavPathOptimization.Domain.Services;
using UnitsNet;

namespace UavPathOptimization.Application.UseCases.Schedule.Queries;

internal sealed class CreateScheduleQueryHandler : IRequestHandler<CreateScheduleQuery, ErrorOr<UavScheduleResult>>
{
    private readonly IUavScheduleCreatorService _uavScheduleCreatorService;
    private readonly IUavModelRepository _uavModelRepository;
    private readonly IAbrasScheduleCreator _abrasScheduleCreator;

    public CreateScheduleQueryHandler(IUavScheduleCreatorService uavScheduleCreatorService,
        IUavModelRepository uavModelRepository,
        IAbrasScheduleCreator abrasScheduleCreator)
    {
        _uavScheduleCreatorService = uavScheduleCreatorService;
        _uavModelRepository = uavModelRepository;
        _abrasScheduleCreator = abrasScheduleCreator;
    }

    public async Task<ErrorOr<UavScheduleResult>> Handle(CreateScheduleQuery request,
        CancellationToken cancellationToken)
    {
        // UAV schedules
        var schedulesResult = await CalculateSchedules(request, cancellationToken);
        if (schedulesResult.IsError)
        {
            return schedulesResult.Errors;
        }

        // ABRAS schedule
        var abrasScheduleResult = _abrasScheduleCreator.CreateScheduleForAbras(
            schedulesResult.Value,
            request.AbrasSpeed,
            request.AbrasDepotLocation);

        if (abrasScheduleResult.IsError)
        {
            return abrasScheduleResult.Errors;
        }

        return abrasScheduleResult.Value;
    }

    private async Task<ErrorOr<IList<UavSchedule>>> CalculateSchedules(CreateScheduleQuery request,
        CancellationToken cancellationToken)
    {
        var schedules = new List<UavSchedule>();

        foreach (var path in request.Paths)
        {
            var uav = await _uavModelRepository.GetByIdAsync(path.UavModelId, cancellationToken);

            if (uav is null)
            {
                return Errors.UavModelErrors.UavModelNotFound;
            }

            var schedulePathResult = _uavScheduleCreatorService.CreateScheduleForUavPath(path, request.DepartureTimeStart,
                request.MonitoringTime, request.ChargingTime, uav);

            if (schedulePathResult.IsError)
            {
                return schedulePathResult.Errors;
            }

            schedules.Add(schedulePathResult.Value);
        }

        return schedules;
    }
}