using ErrorOr;
using MediatR;
using UavPathOptimization.Application.Common.Authentication;
using UavPathOptimization.Application.Common.Persistence.User;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities.Results;

namespace UavPathOptimization.Application.UseCases.Authentication.Queries.Login;

public sealed class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _tokenGenerator;

    private readonly IMediator _mediator;

    public LoginQueryHandler(IJwtTokenGenerator tokenGenerator, IMediator mediator)
    {
        _tokenGenerator = tokenGenerator;
        _mediator = mediator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        // 1. Validate fields
        // TODO - add validation
        // 2. Check if user exists
        var userResult = await _mediator.Send(new GetUserByEmailQuery(request.Email), cancellationToken);
        if (userResult.IsError)
        {
            return Errors.Login.UserNotFound;
        }

        // 3. Check if password is correct
        var checkUserPasswordQuery = new CheckUserPasswordQuery(userResult.Value, request.Password);
        var isPasswordCorrect = await _mediator.Send(checkUserPasswordQuery, cancellationToken);

        if (!isPasswordCorrect)
        {
            return Errors.Login.PasswordMismatch;
        }

        // 4. Generate token
        var token = _tokenGenerator.GenerateToken(userResult.Value);

        return new AuthenticationResult(userResult.Value, token);
    }
}