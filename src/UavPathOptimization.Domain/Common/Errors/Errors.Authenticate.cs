using ErrorOr;

namespace UavPathOptimization.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authenticate
    {
        public static Error DefaultError = Error.Failure(
            code: "Authenticate.DefaultError",
            description: "An error occurred."
        );

        public static Error ConcurrencyFailure = Error.Failure(
            code: "Authenticate.ConcurrencyFailure",
            description: "Concurrency failure."
        );

        public static Error PasswordMismatch = Error.Validation(
            code: "Authenticate.PasswordMismatch",
            description: "Invalid password."
        );

        public static Error InvalidToken = Error.Validation(
            code: "Authenticate.InvalidToken",
            description: "Invalid token."
        );

        public static Error RecoveryCodeRedemptionFailed = Error.Failure(
            code: "Authenticate.RecoveryCodeRedemptionFailed",
            description: "Recovery code redemption failed."
        );

        public static Error LoginAlreadyAssociated = Error.Validation(
            code: "Authenticate.LoginAlreadyAssociated",
            description: "Login already associated with another user."
        );

        public static Error InvalidUserName = Error.Validation(
            code: "Authenticate.InvalidUserName",
            description: "Invalid username."
        );

        public static Error InvalidEmail = Error.Validation(
            code: "Authenticate.InvalidEmail",
            description: "Invalid email address."
        );

        public static Error DuplicateUserName = Error.Conflict(
            code: "Authenticate.DuplicateUserName",
            description: "Username already taken."
        );

        public static Error DuplicateEmail = Error.Conflict(
            code: "Authenticate.DuplicateEmail",
            description: "Email address already associated with another user."
        );

        public static Error InvalidRoleName = Error.Validation(
            code: "Authenticate.InvalidRoleName",
            description: "Invalid role name."
        );

        public static Error DuplicateRoleName = Error.Conflict(
            code: "Authenticate.DuplicateRoleName",
            description: "Role name already taken."
        );

        public static Error UserAlreadyHasPassword = Error.Validation(
            code: "Authenticate.UserAlreadyHasPassword",
            description: "User already has a password."
        );

        public static Error UserLockoutNotEnabled = Error.Failure(
            code: "Authenticate.UserLockoutNotEnabled",
            description: "User lockout is not enabled."
        );

        public static Error UserAlreadyInRole = Error.Conflict(
            code: "Authenticate.UserAlreadyInRole",
            description: "User is already in the specified role."
        );

        public static Error UserNotInRole = Error.Validation(
            code: "Authenticate.UserNotInRole",
            description: "User is not in the specified role."
        );

        public static Error PasswordTooShort = Error.Validation(
            code: "Authenticate.PasswordTooShort",
            description: "Password is too short."
        );

        public static Error PasswordRequiresUniqueChars = Error.Validation(
            code: "Authenticate.PasswordRequiresUniqueChars",
            description: "Password requires unique characters."
        );

        public static Error PasswordRequiresNonAlphanumeric = Error.Validation(
            code: "Authenticate.PasswordRequiresNonAlphanumeric",
            description: "Password requires non-alphanumeric characters."
        );

        public static Error PasswordRequiresLower = Error.Validation(
            code: "Authenticate.PasswordRequiresLower",
            description: "Password requires lowercase characters."
        );

        public static Error PasswordRequiresUpper = Error.Validation(
            code: "Authenticate.PasswordRequiresUpper",
            description: "Password requires uppercase characters."
        );
    }
}