using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities.Results;
using UavPathOptimization.Domain.Entities.UavEntities;
using UavPathOptimization.Domain.Repositories;
using UavPathOptimization.Domain.Services;

namespace UavPathOptimization.Application.UseCases.Schedule.Queries;

internal sealed class CreateScheduleQueryHandler : IRequestHandler<CreateScheduleQuery, ErrorOr<UavScheduleResult>>
{
    private readonly IUavScheduleCreator _uavScheduleCreator;
    private readonly IUavModelRepository _uavModelRepository;

    public CreateScheduleQueryHandler(IUavScheduleCreator uavScheduleCreator, IUavModelRepository uavModelRepository)
    {
        _uavScheduleCreator = uavScheduleCreator;
        _uavModelRepository = uavModelRepository;
    }

    public async Task<ErrorOr<UavScheduleResult>> Handle(CreateScheduleQuery request, CancellationToken cancellationToken)
    {
        // UAV schedules
        var schedules = new List<UavPathSchedule>();

        foreach (var path in request.Paths)
        {
            var uav = await _uavModelRepository.GetByIdAsync(path.UavModelId, cancellationToken);

            if (uav is null)
            {
                return Errors.UavModelErrors.UavModelNotFound;
            }

            var schedulePathResult = _uavScheduleCreator.CreateScheduleForUavPath(path, request.DepartureTimeStart, request.MonitoringTime, request.ChargingTime, uav);

            if (schedulePathResult.IsError)
            {
                return schedulePathResult.Errors;
            }

            schedules.Add(schedulePathResult.Value);
        }

        // TODO ABRAS schedules

        return new UavScheduleResult(schedules, new List<UavScheduleEntry>());
    }
}