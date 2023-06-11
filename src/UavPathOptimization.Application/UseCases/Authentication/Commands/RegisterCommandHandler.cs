using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Authentication;
using UavPathOptimization.Application.Common.Persistance;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Contracts.Authentication;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.UseCases.Authentication.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResponse>>
{
    private readonly IJwtTokenGenerator _tokenGenerator;

    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IJwtTokenGenerator tokenGenerator, IUserRepository userRepository)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
    }

    public Task<ErrorOr<AuthenticationResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate fields

        // 2. Check if user already exists
        if (_userRepository.GetUserByEmail(request.Email) != null)
        {
            return Task.FromResult<ErrorOr<AuthenticationResponse>>(Errors.Authenticate.UserAlreadyExists);
        }

        // 3. Create user
        var user = new User()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password
        };

        // 4. Generate token
        var token = _tokenGenerator.GenerateToken(user.Id, request.FirstName, request.LastName, request.Email);

        var response = new AuthenticationResponse(
            user.Id,
            request.FirstName,
            request.LastName,
            request.Email,
            token
        );

        return Task.FromResult<ErrorOr<AuthenticationResponse>>(response);
    }
}