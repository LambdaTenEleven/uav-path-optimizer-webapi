using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Authentication;
using UavPathOptimization.Domain.Contracts.Authentication;

namespace UavPathOptimization.Application.UseCases.Authentication.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResponse>>
{
    private readonly IJwtTokenGenerator _tokenGenerator;

    public RegisterCommandHandler(IJwtTokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
    }

    public Task<ErrorOr<AuthenticationResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists

        // Create user

        // Generate token
        var userId = Guid.NewGuid();
        var token = _tokenGenerator.GenerateToken(userId, request.FirstName, request.LastName, request.Email);

        var response = new AuthenticationResponse(
            userId,
            request.FirstName,
            request.LastName,
            request.Email,
            token
        );

        return Task.FromResult<ErrorOr<AuthenticationResponse>>(response);
    }
}