using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace UavPathOptimization.WebAPI.Extensions;

public static class ErrorOrExtensions
{
    public static IActionResult GetActionResultFromError<T>(this ErrorOr<T> errorOr)
    {
        var firstErrorDescription = errorOr.FirstError.Description;
        return errorOr.FirstError.Type switch
        {
            ErrorType.Failure => new BadRequestObjectResult(firstErrorDescription),
            ErrorType.Unexpected => new BadRequestObjectResult(firstErrorDescription),
            ErrorType.Validation => new BadRequestObjectResult(firstErrorDescription),
            ErrorType.Conflict => new ConflictObjectResult(firstErrorDescription),
            ErrorType.NotFound => new NotFoundObjectResult(firstErrorDescription),
            _ => new StatusCodeResult(500)
        };
    }
}