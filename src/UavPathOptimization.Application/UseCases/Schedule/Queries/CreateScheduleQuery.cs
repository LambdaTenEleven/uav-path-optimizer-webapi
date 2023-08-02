using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities.Results;

namespace UavPathOptimization.Application.UseCases.Schedule.Queries;

public record CreateScheduleQuery(
    IList<UavPathDto> Paths,
    DateTime DepartureTimeStart,
    TimeSpan MonitoringTime,
    TimeSpan ChargingTime,
    double AbrasSpeed
) : IRequest<ErrorOr<UavScheduleResult>>;