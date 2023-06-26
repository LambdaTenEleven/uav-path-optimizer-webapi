using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities;
using UavPathOptimization.Domain.Entities.Results;

namespace UavPathOptimization.Application.UseCases.Authentication.Commands.Register;

public record RegisterCommand(
    string UserName,
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;