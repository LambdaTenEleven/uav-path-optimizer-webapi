using MediatR;
using Microsoft.AspNetCore.Mvc;
using UavPathOptimization.Application.UseCases.Authentication.Commands;
using UavPathOptimization.Domain.Contracts.Authentication;

namespace UavPathOptimization.WebAPI.Controllers;

[Route("api/auth")]
public class AuthenticationController : ApiController
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = new RegisterCommand(
            request.UserName,
            request.Email,
            request.Password
        );

        var result = await _mediator.Send(command);

        return result.Match(
            result => Ok(new AuthenticationResponse(
                    result.Id,
                    result.Token
                )),
            errors => Problem(errors)
        );
    }

    // [HttpPost("login")]
    // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> Login([FromBody] LoginRequest request)
    // {
    //     var command = new LoginCommand(request.Email, request.Password);
    //     var result = await _mediator.Send(command);
    //
    //     return result.Match(
    //         result => Ok(new LoginResponse(result)),
    //         errors => Problem(errors)
    //     );
    // }
}