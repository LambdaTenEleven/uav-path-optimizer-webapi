using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Authentication;
using UavPathOptimization.Application.Common.Persistence.User;
using UavPathOptimization.Domain.Entities;
using UavPathOptimization.Domain.Entities.Results;

namespace UavPathOptimization.Application.UseCases.Authentication.Commands.Register;

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
        // 1. Create user
        var user = new User()
        {
            UserName = request.UserName,
            Email = request.Email
        };

        // 2. Add user to database using AddUserCommand that uses UserManager
        var result = await _mediator.Send(new AddUserToDbCommand(user, request.Password), cancellationToken);
        if (result.IsError)
        {
            return result.Errors;
        }

        // 3. Generate token
        var token = _tokenGenerator.GenerateToken(user);

        var authenticationResult = new AuthenticationResult(
            user,
            token
        );

        return authenticationResult;
    }
}