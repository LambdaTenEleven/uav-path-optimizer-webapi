using ErrorOr;

namespace UavPathOptimization.Domain.Common.Errors;

public partial class Errors
{
    public static class Login
    {
        public static Error UserNotFound = Error.NotFound(
            code: "Authenticate.UserNotFound",
            description: "User not found."
        );

        public static Error PasswordMismatch = Error.Validation(
            code: "Authenticate.PasswordMismatch",
            description: "Invalid password."
        );
    }
}