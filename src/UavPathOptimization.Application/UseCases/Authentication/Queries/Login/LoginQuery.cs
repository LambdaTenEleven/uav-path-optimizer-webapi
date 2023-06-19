using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.UseCases.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;