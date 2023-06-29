using FluentValidation;
using Microsoft.Extensions.Options;
using UavPathOptimization.Domain.Common.Settings;

namespace UavPathOptimization.Application.UseCases.Authentication.Commands.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly IOptions<PasswordSettings> _passwordSettings;

    public RegisterCommandValidator(IOptions<PasswordSettings> passwordSettings)
    {
        _passwordSettings = passwordSettings;

        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(_passwordSettings.Value.RequiredLength);

        if (_passwordSettings.Value.RequireDigit)
        {
            RuleFor(x => x.Password)
                .Must(password => password.Any(char.IsDigit))
                .WithMessage("The password must contain at least one digit.");
        }

        if (_passwordSettings.Value.RequireLowercase)
        {
            RuleFor(x => x.Password)
                .Must(password => password.Any(char.IsLower))
                .WithMessage("The password must contain at least one lowercase letter.");
        }

        if (_passwordSettings.Value.RequireUppercase)
        {
            RuleFor(x => x.Password)
                .Must(password => password.Any(char.IsUpper))
                .WithMessage("The password must contain at least one uppercase letter.");
        }

        if (_passwordSettings.Value.RequireNonAlphanumeric)
        {
            RuleFor(x => x.Password)
                .Must(password => password.Any(IsNonAlphanumeric))
                .WithMessage("The password must contain at least one non-alphanumeric character.");
        }
    }

    private bool IsNonAlphanumeric(char c)
    {
        return !char.IsLetterOrDigit(c);
    }
}