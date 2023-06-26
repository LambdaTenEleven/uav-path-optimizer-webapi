namespace UavPathOptimization.Domain.Contracts.UavModel;

public record CreateUavModelRequest(
    string Name,
    double MaxSpeed,
    TimeSpan MaxFlightTime
);