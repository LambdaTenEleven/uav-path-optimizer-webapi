using ErrorOr;

namespace UavPathOptimization.Domain.Common.Errors;

public partial class Errors
{
    public static class UavModelErrors
    {
        public static readonly Error UavModelNotCreated = Error.Failure(
            code: "UavModel.NotCreated",
            description: "Uav model could not be created."
        );

        public static readonly Error UavModelNotFound = Error.NotFound(
            code: "UavModel.NotFound",
            description: "Uav model could not be found."
        );
    }
}