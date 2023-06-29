namespace UavPathOptimization.Domain.Contracts.UavModel;

public sealed record UpdateUavModelRequest(
    string Name,
    double MaxSpeed,
    TimeSpan MaxFlightTime
);