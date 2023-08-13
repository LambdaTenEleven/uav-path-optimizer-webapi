using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using UavPathOptimization.Application.Common.Services;
using UavPathOptimization.Domain.Common.LoggerDefinitions;

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
        _logger.LogRequestStart(typeof(TRequest).Name, _dateTimeProvider.UtcNow);

        var response = await next();
        if (response is IErrorOr { IsError: true } errorOr)
        {
            _logger.LogRequestError(nameof(LoggingPipelineBehaviour<TRequest, TResponse>), _dateTimeProvider.UtcNow, errorOr.Errors.First());
        }

        _logger.LogRequestEnd(typeof(TRequest).Name, _dateTimeProvider.UtcNow);

        return response;
    }
}