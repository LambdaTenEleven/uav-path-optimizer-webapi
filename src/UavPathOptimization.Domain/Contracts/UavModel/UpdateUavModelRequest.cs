namespace UavPathOptimization.Domain.Contracts.UavModel;

public record UpdateUavModelRequest(
    string Name,
    double MaxSpeed,
    TimeSpan MaxFlightTime
);