namespace UavPathOptimization.Domain.Contracts.Authentication;

public record LoginRequest(
    string Email,
    string Password
);