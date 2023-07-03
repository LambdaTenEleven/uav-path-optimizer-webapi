using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities.Results;

namespace UavPathOptimization.Application.UseCases.Schedule.Queries;

public record ScheduleCreatorQuery(
    Guid UavModelId,
    IList<GeoCoordinateDto> Path,
    DateTime DepartureTimeStart,
    TimeSpan MonitoringTime,
    TimeSpan ChargingTime,
    double AbrasSpeed
) : IRequest<ErrorOr<UavScheduleResult>>;