using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Authentication;
using UavPathOptimization.Application.Common.Persistence;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.UseCases.Authentication.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _tokenGenerator;

    private readonly IMediator _mediator;

    public RegisterCommandHandler(IJwtTokenGenerator tokenGenerator, IMediator mediator)
    {
        _tokenGenerator = tokenGenerator;
        _mediator = mediator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate fields
        // TODO: Validate fields
        // 2. Create user
        var user = new User()
        {
            UserName = request.UserName,
            Email = request.Email
        };

        // 3. Add user to database using AddUserCommand that uses UserManager
        var result = await _mediator.Send(new AddUserCommand(user, request.Password), cancellationToken);
        if (result.IsError)
        {
            return result.Errors;
        }

        // 4. Generate token
        var token = _tokenGenerator.GenerateToken(user);

        var authenticationResult = new AuthenticationResult(
            user,
            token
        );

        return authenticationResult;
    }
}