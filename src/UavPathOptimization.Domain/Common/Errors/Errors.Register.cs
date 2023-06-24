using ErrorOr;

namespace UavPathOptimization.Domain.Common.Errors;

public static partial class Errors
{
    public static class Register
    {
        public static readonly Error DefaultError = Error.Failure(
            code: "Authenticate.DefaultError",
            description: "An error occurred."
        );

        public static readonly Error ConcurrencyFailure = Error.Failure(
            code: "Authenticate.ConcurrencyFailure",
            description: "Concurrency failure."
        );

        public static readonly Error PasswordMismatch = Error.Validation(
            code: "Authenticate.PasswordMismatch",
            description: "Invalid password."
        );

        public static readonly Error InvalidToken = Error.Validation(
            code: "Authenticate.InvalidToken",
            description: "Invalid token."
        );

        public static readonly Error RecoveryCodeRedemptionFailed = Error.Failure(
            code: "Authenticate.RecoveryCodeRedemptionFailed",
            description: "Recovery code redemption failed."
        );

        public static readonly Error LoginAlreadyAssociated = Error.Validation(
            code: "Authenticate.LoginAlreadyAssociated",
            description: "Login already associated with another user."
        );

        public static readonly Error InvalidUserName = Error.Validation(
            code: "Authenticate.InvalidUserName",
            description: "Invalid username."
        );

        public static readonly Error InvalidEmail = Error.Validation(
            code: "Authenticate.InvalidEmail",
            description: "Invalid email address."
        );

        public static readonly Error DuplicateUserName = Error.Conflict(
            code: "Authenticate.DuplicateUserName",
            description: "Username already taken."
        );

        public static readonly Error DuplicateEmail = Error.Conflict(
            code: "Authenticate.DuplicateEmail",
            description: "Email address already associated with another user."
        );

        public static readonly Error InvalidRoleName = Error.Validation(
            code: "Authenticate.InvalidRoleName",
            description: "Invalid role name."
        );

        public static readonly Error DuplicateRoleName = Error.Conflict(
            code: "Authenticate.DuplicateRoleName",
            description: "Role name already taken."
        );

        public static readonly Error UserAlreadyHasPassword = Error.Validation(
            code: "Authenticate.UserAlreadyHasPassword",
            description: "User already has a password."
        );

        public static readonly Error UserLockoutNotEnabled = Error.Failure(
            code: "Authenticate.UserLockoutNotEnabled",
            description: "User lockout is not enabled."
        );

        public static readonly Error UserAlreadyInRole = Error.Conflict(
            code: "Authenticate.UserAlreadyInRole",
            description: "User is already in the specified role."
        );

        public static readonly Error UserNotInRole = Error.Validation(
            code: "Authenticate.UserNotInRole",
            description: "User is not in the specified role."
        );

        public static readonly Error PasswordTooShort = Error.Validation(
            code: "Authenticate.PasswordTooShort",
            description: "Password is too short."
        );

        public static readonly Error PasswordRequiresUniqueChars = Error.Validation(
            code: "Authenticate.PasswordRequiresUniqueChars",
            description: "Password requires unique characters."
        );

        public static readonly Error PasswordRequiresNonAlphanumeric = Error.Validation(
            code: "Authenticate.PasswordRequiresNonAlphanumeric",
            description: "Password requires non-alphanumeric characters."
        );

        public static readonly Error PasswordRequiresLower = Error.Validation(
            code: "Authenticate.PasswordRequiresLower",
            description: "Password requires lowercase characters."
        );

        public static readonly Error PasswordRequiresUpper = Error.Validation(
            code: "Authenticate.PasswordRequiresUpper",
            description: "Password requires uppercase characters."
        );
    }
}