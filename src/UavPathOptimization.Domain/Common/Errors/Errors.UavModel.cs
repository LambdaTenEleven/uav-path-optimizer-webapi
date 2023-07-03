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

        public static readonly Error UavModelNotUpdated = Error.Failure(
            code: "UavModel.NotUpdated",
            description: "Uav model could not be updated."
        );

        public static readonly Error UavModelDeletionFailed = Error.Failure(
            code: "UavModel.DeletionFailed",
            description: "Uav model could not be deleted."
        );

        public static readonly Error UavModelNotFound = Error.NotFound(
            code: "UavModel.NotFound",
            description: "Uav model could not be found."
        );

        public static readonly Error UavModelNameAlreadyExist = Error.Conflict(
            code: "UavModel.NameAlreadyExist",
            description: "Uav model with this name already exist."
        );

        public static readonly Error UavModelMaxFlightTimeExceeded = Error.Failure(
            code: "UavModel.MaxFlightTimeExceeded",
            description: "Uav model max flight time exceeded."
        );
    }
}