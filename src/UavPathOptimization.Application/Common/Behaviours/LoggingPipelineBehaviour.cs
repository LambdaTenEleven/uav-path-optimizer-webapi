using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using UavPathOptimization.Application.Common.Services;

namespace UavPathOptimization.Application.Common.Behaviours;

public class LoggingPipelineBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public LoggingPipelineBehaviour(ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger, IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting request {@RequestName}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            _dateTimeProvider.UtcNow);

        var response = await next();
        if (response is IErrorOr { IsError: true } errorOr)
        {
            _logger.LogError(
                "Request error {@RequestName}, {@DateTimeUtc}, {@Error}",
                typeof(TRequest).Name,
                _dateTimeProvider.UtcNow,
                errorOr.Errors.First().Description);
        }

        _logger.LogInformation(
            "Finished request {@RequestName}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            _dateTimeProvider.UtcNow);

        return response;
    }
}