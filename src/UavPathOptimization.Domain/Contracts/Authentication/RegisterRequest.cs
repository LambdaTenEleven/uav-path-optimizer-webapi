namespace UavPathOptimization.Domain.Contracts.Authentication;

public sealed record RegisterRequest(
    string UserName,
    string Email,
    string Password
);