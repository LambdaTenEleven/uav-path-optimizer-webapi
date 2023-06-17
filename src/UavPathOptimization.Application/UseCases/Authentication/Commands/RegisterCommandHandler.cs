using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Authentication;
using UavPathOptimization.Application.Common.Persistence;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Contracts.Authentication;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.UseCases.Authentication.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResponse>>
{
    private readonly IJwtTokenGenerator _tokenGenerator;

    private readonly IMediator _mediator;

    public RegisterCommandHandler(IJwtTokenGenerator tokenGenerator, IMediator mediator)
    {
        _tokenGenerator = tokenGenerator;
        _mediator = mediator;
    }

    public Task<ErrorOr<AuthenticationResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate fields

        // 2. Check if user already exists
        // if (_userRepository.GetUserByEmail(request.Email) != null)
        // {
        //     return Task.FromResult<ErrorOr<AuthenticationResponse>>(Errors.Authenticate.UserAlreadyExists);
        // }

        // 3. Create user
        var user = new User()
        {
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = request.Password // TODO: Hash password
        };

        var result = _mediator.Send(new AddUserCommand(user), cancellationToken);

        // 4. Generate token
        var token = _tokenGenerator.GenerateToken(user);

        var response = new AuthenticationResponse(
            user.Id,
            token
        );

        return Task.FromResult<ErrorOr<AuthenticationResponse>>(response);
    }
}