using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UavPathOptimization.Application.Common.Services;

namespace UavPathOptimization.Application.Common.Behaviours;

public class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;
    private readonly ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ValidationPipelineBehaviour(ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> logger, IDateTimeProvider dateTimeProvider, IValidator<TRequest>? validator = null)
    {
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            _logger.LogWarning(
                "No validator found for request {@RequestName}, {@DateTimeUtc}",
                typeof(TRequest).Name,
                _dateTimeProvider.UtcNow);

            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        _logger.LogError(
            "Request validation error {@RequestName}, {@DateTimeUtc}, {@Errors}",
            typeof(TRequest).Name,
            _dateTimeProvider.UtcNow,
            validationResult.Errors);

        var errors =
            validationResult.Errors.ConvertAll(failure => Error.Validation(failure.PropertyName, failure.ErrorMessage));

        return (dynamic)errors;
    }
}