namespace UavPathOptimization.Domain.Contracts.Authentication;

public sealed record AuthenticationResponse(
    Guid Id,
    string Token
);