using ErrorOr;

namespace UavPathOptimization.Domain.Common.Errors;

public partial class Errors
{
    public static class UavModel
    {
        public static readonly Error UavModelNotCreated = Error.Failure(
            code: "UavModel.NotCreated",
            description: "Uav model could not be created."
        );
    }
}