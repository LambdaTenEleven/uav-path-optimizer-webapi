namespace UavPathOptimization.Domain.Contracts.UavModel;

public record UavModelResponse(
    Guid Id,
    string Name,
    double MaxSpeed,
    TimeSpan MaxFlightTime
);