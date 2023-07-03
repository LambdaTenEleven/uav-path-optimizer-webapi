namespace UavPathOptimization.Domain.Contracts.Schedule;

public record UavScheduleRequest(
    Guid UavModelId,
    IList<GeoCoordinateDto> Path,
    DateTime DepartureTimeStart,
    TimeSpan MonitoringTime,
    TimeSpan ChargingTime,
    double AbrasSpeed
);