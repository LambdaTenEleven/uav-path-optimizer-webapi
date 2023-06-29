namespace UavPathOptimization.Domain.Contracts.Authentication;

public sealed record LoginRequest(
    string Email,
    string Password
);