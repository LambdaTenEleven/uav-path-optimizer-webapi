namespace UavPathOptimization.Domain.Contracts.UavModel;

public sealed record CreateUavModelRequest(
    string Name,
    double MaxSpeed,
    TimeSpan MaxFlightTime
);