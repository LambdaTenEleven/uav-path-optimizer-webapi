using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using UavPathOptimization.WebAPI.Common.Http;

namespace UavPathOptimization.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;

        var firsterror = errors[0];
        var statusCode = firsterror.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: firsterror.Description);
    }
}