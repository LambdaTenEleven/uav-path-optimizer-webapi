using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Contracts.Authentication;

namespace UavPathOptimization.Application.UseCases.Authentication.Commands;

public record RegisterCommand(
    string UserName,
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResponse>>;