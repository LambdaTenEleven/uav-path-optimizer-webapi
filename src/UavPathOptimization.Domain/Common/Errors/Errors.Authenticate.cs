using ErrorOr;

namespace UavPathOptimization.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authenticate
    {
        public static Error UserAlreadyExists = Error.Validation(
            code: "Authenticate.UserAlreadyExists",
            description: "User already exists."
        );
    }
}