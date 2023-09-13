using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities.Results;
using UavPathOptimization.Domain.Entities.UavEntities;
using UavPathOptimization.Domain.Repositories;
using UavPathOptimization.Domain.Services;

namespace UavPathOptimization.Application.UseCases.Schedule.Queries;

internal sealed class CreateScheduleQueryHandler : IRequestHandler<CreateScheduleQuery, ErrorOr<UavScheduleResult>>
{
    private readonly IUavScheduleCreatorService _uavScheduleCreatorService;
    private readonly IUavModelRepository _uavModelRepository;
    private readonly ILogger<CreateScheduleQueryHandler> _logger;

    public CreateScheduleQueryHandler(IUavScheduleCreatorService uavScheduleCreatorService, IUavModelRepository uavModelRepository, ILogger<CreateScheduleQueryHandler> logger)
    {
        _uavScheduleCreatorService = uavScheduleCreatorService;
        _uavModelRepository = uavModelRepository;
        _logger = logger;
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

            var schedulePathResult = _uavScheduleCreatorService.CreateScheduleForUavPath(path, request.DepartureTimeStart, request.MonitoringTime, request.ChargingTime, uav);

            if (schedulePathResult.IsError)
            {
                return schedulePathResult.Errors;
            }

            schedules.Add(schedulePathResult.Value);
        }

#pragma warning disable CA1848
#pragma warning disable CA2254
        _logger.LogInformation(message: $"ABRAS Coords: {request.AbrasDepotLocation.Latitude}, {request.AbrasDepotLocation.Longitude}");
#pragma warning restore CA2254
#pragma warning restore CA1848

        // TODO ABRAS schedules

        return new UavScheduleResult(schedules, new List<UavScheduleEntry>());
    }
}