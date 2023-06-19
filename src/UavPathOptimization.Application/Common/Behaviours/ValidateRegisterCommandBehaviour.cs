using ErrorOr;
using FluentValidation;
using MediatR;
using UavPathOptimization.Application.UseCases.Authentication.Commands;
using UavPathOptimization.Application.UseCases.Authentication.Commands.Register;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.Common.Behaviours;

public class ValidateRegisterCommandBehaviour : IPipelineBehavior<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IValidator<RegisterCommand> _validator;

    public ValidateRegisterCommandBehaviour(IValidator<RegisterCommand> validator)
    {
        _validator = validator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request,
        RequestHandlerDelegate<ErrorOr<AuthenticationResult>> next, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors =
            validationResult.Errors.ConvertAll(failure => Error.Validation(failure.PropertyName, failure.ErrorMessage));

        return errors;

    }
}