using ErrorOr;
using Microsoft.Extensions.Logging;

namespace UavPathOptimization.Domain.Common.LoggerDefinitions;

public static class LoggerPipelineDefinitions
{
    private static readonly Action<ILogger, string, DateTime, Exception?> RequestStartDefinition = LoggerMessage.Define<string, DateTime>(
        LogLevel.Information,
        0,
        "Starting request {@RequestName}, {@DateTimeUtc}");

    private static readonly Action<ILogger, string, DateTime, Exception?> RequestEndDefinition = LoggerMessage.Define<string, DateTime>(
        LogLevel.Information,
        1,
        "Finished request {@RequestName}, {@DateTimeUtc}");

    private static readonly Action<ILogger, string, DateTime, Error, Exception?> RequestErrorDefinition = LoggerMessage.Define<string, DateTime, Error>(
        LogLevel.Error,
        2,
        "Request errors {@RequestName}, {@DateTimeUtc}, {@FirstError}");

    public static void LogRequestStart(this ILogger logger, string requestName, DateTime dateTimeUtc)
    {
        RequestStartDefinition(logger, requestName, dateTimeUtc, null);
    }

    public static void LogRequestEnd(this ILogger logger, string requestName, DateTime dateTimeUtc)
    {
        RequestEndDefinition(logger, requestName, dateTimeUtc, null);
    }

    public static void LogRequestError(this ILogger logger, string requestName, DateTime dateTimeUtc, Error firstError)
    {
        RequestErrorDefinition(logger, requestName, dateTimeUtc, firstError, null);
    }
}