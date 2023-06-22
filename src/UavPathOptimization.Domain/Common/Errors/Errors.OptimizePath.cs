using ErrorOr;

namespace UavPathOptimization.Domain.Common.Errors;

public static partial class Errors
{
    public static class OptimizePath
    {
        public static Error InputPathValidationError = Error.Validation(
            code: "OptimizePath.InputPathValidationError",
            description: "Path must include more than 2 GeoCoordinate."
        );

        public static Error SolutionError = Error.Validation(
            code: "OptimizePath.SolutionError",
            description: "Solution error."
        );
    }
}