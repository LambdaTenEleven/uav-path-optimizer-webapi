namespace UavPathOptimization.Domain.Contracts.Authentication;

public record AuthenticationResponse(
    Guid Id,
    string Token
);