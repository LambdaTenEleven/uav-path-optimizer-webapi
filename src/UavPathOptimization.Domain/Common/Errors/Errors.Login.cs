using ErrorOr;

namespace UavPathOptimization.Domain.Common.Errors;

public partial class Errors
{
    public static class Login
    {
        public static readonly Error UserNotFound = Error.NotFound(
            code: "Authenticate.UserNotFound",
            description: "User not found."
        );

        public static readonly Error PasswordMismatch = Error.Validation(
            code: "Authenticate.PasswordMismatch",
            description: "Invalid password."
        );
    }
}