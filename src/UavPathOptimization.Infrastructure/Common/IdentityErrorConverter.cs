using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace UavPathOptimization.Infrastructure.Common;

public static class IdentityErrorConverter
{
    public static Error Convert(IdentityError identityError)
    {
        ErrorType errorType;

        switch (identityError.Code)
        {
            case "ConcurrencyFailure":
                errorType = ErrorType.Failure;
                break;
            case "PasswordMismatch":
            case "InvalidToken":
            case "RecoveryCodeRedemptionFailed":
            case "LoginAlreadyAssociated":
            case "InvalidUserName":
            case "InvalidEmail":
            case "InvalidRoleName":
            case "UserAlreadyHasPassword":
            case "UserLockoutNotEnabled":
            case "UserAlreadyInRole":
            case "UserNotInRole":
            case "PasswordTooShort":
            case "PasswordRequiresUniqueChars":
            case "PasswordRequiresNonAlphanumeric":
            case "PasswordRequiresLower":
            case "PasswordRequiresUpper":
                errorType = ErrorType.Validation;
                break;
            case "DuplicateUserName":
            case "DuplicateEmail":
            case "DuplicateRoleName":
                errorType = ErrorType.Conflict;
                break;
            default:
                errorType = ErrorType.Failure;
                break;
        }

        return ErrorFactory(errorType, "Authentication." + identityError.Code, identityError.Description);
    }

    private static Error ErrorFactory(ErrorType errorType, string code, string description)
    {
        return errorType switch
        {
            ErrorType.Failure => Error.Failure(code, description),
            ErrorType.Unexpected => Error.Unexpected(code, description),
            ErrorType.Validation => Error.Validation(code, description),
            ErrorType.Conflict => Error.Conflict(code, description),
            ErrorType.NotFound => Error.NotFound(code, description),
            _ => Error.Unexpected(code, description)
        };
    }
}