using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Contracts.Authentication;

namespace UavPathOptimization.Application.UseCases.Authentication.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResponse>>
{
    public Task<ErrorOr<AuthenticationResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var response = new AuthenticationResponse(
            Guid.NewGuid(),
            request.FirstName,
            request.LastName,
            request.Email,
            "token"
        );

        return Task.FromResult<ErrorOr<AuthenticationResponse>>(response);
    }
}