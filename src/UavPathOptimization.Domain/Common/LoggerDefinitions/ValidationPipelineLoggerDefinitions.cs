using ErrorOr;
using Microsoft.Extensions.Logging;

namespace UavPathOptimization.Domain.Common.LoggerDefinitions;

public static class ValidationPipelineLoggerDefinitions
{
    private static readonly Action<ILogger, string, DateTime, Exception?> ValidatorNotFoundDefinition = LoggerMessage.Define<string, DateTime>(
        LogLevel.Warning,
        0,
        "No validator found for request {RequestName}, {DateTimeUtc}");

    private static readonly Action<ILogger, string, DateTime, Error, Exception?> RequestValidationErrorsDefinition = LoggerMessage.Define<string, DateTime, Error>(
        LogLevel.Error,
        1,
        "Request validation error {RequestName}, {DateTimeUtc}, {Error}");

    public static void LogValidatorNotFound(this ILogger logger, string requestName, DateTime dateTimeUtc)
    {
        ValidatorNotFoundDefinition(logger, requestName, dateTimeUtc, null);
    }

    public static void LogRequestValidationErrors(this ILogger logger, string requestName, DateTime dateTimeUtc, Error firstError)
    {
        RequestValidationErrorsDefinition(logger, requestName, dateTimeUtc, firstError, null);
    }
}