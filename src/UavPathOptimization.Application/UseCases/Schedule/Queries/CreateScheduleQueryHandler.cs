using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities.Results;
using UavPathOptimization.Domain.Entities.Schedule;
using UavPathOptimization.Domain.Repositories;
using UavPathOptimization.Domain.Services;
using UnitsNet;

namespace UavPathOptimization.Application.UseCases.Schedule.Queries;

internal sealed class CreateScheduleQueryHandler : IRequestHandler<CreateScheduleQuery, ErrorOr<UavScheduleResult>>
{
    private readonly IScheduleCreatorService _scheduleCreatorService;
    private readonly IUavModelRepository _uavModelRepository;
    private readonly ILogger<CreateScheduleQueryHandler> _logger;
    private readonly IPathOptimizationService _pathOptimizationService;

    public CreateScheduleQueryHandler(IScheduleCreatorService scheduleCreatorService, IUavModelRepository uavModelRepository, ILogger<CreateScheduleQueryHandler> logger, IPathOptimizationService pathOptimizationService)
    {
        _scheduleCreatorService = scheduleCreatorService;
        _uavModelRepository = uavModelRepository;
        _logger = logger;
        _pathOptimizationService = pathOptimizationService;
    }

    public async Task<ErrorOr<UavScheduleResult>> Handle(CreateScheduleQuery request, CancellationToken cancellationToken)
    {
        // UAV schedules
        var schedules = new List<UavSchedule>();

        foreach (var path in request.Paths)
        {
            var uav = await _uavModelRepository.GetByIdAsync(path.UavModelId, cancellationToken);

            if (uav is null)
            {
                return Errors.UavModelErrors.UavModelNotFound;
            }

            var schedulePathResult = _scheduleCreatorService.CreateScheduleForUavPath(path, request.DepartureTimeStart, request.MonitoringTime, request.ChargingTime, uav);

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
        var speed = Speed.FromKilometersPerHour(request.AbrasSpeed);
        // select PBR entries from all schedule entries and transform to GeoCoordinateDto
        var pbrEntries = schedules
            .SelectMany(x => x.UavScheduleEntries)
            .Where(x => x.IsPBR)
            .Select(x => x.Location)
            .ToList();

        pbrEntries.Insert(0, request.AbrasDepotLocation);

        // calculate optimized path for PBR entries
        // TODO: check for errors
        var optimizedPath = _pathOptimizationService.OptimizePath(1, pbrEntries).Value;

        var abrasSchedules = _scheduleCreatorService.CreateScheduleForAbrasPath(optimizedPath[0].Path, request.DepartureTimeStart, request.ChargingTime, speed, request.AbrasDepotLocation);

        return new UavScheduleResult(schedules, abrasSchedules.Value);
    }
}