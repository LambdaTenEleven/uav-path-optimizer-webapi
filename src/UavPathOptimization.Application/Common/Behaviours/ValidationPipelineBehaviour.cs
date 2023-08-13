using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UavPathOptimization.Application.Common.Services;
using UavPathOptimization.Domain.Common.LoggerDefinitions;

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
            _logger.LogValidatorNotFound(typeof(TRequest).Name, _dateTimeProvider.UtcNow);

            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors =
            validationResult.Errors.ConvertAll(failure => Error.Validation(failure.PropertyName, failure.ErrorMessage));

        _logger.LogRequestValidationErrors(typeof(TRequest).Name, _dateTimeProvider.UtcNow, errors.First());

        return (dynamic)errors;
    }
}