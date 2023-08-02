namespace UavPathOptimization.Domain.Contracts.Schedule;

public record UavScheduleRequest(
    IList<UavPathDto> Paths,
    DateTime DepartureTimeStart,
    TimeSpan MonitoringTime,
    TimeSpan ChargingTime,
    double AbrasSpeed
);