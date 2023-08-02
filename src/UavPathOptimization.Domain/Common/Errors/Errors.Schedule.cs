using ErrorOr;

namespace UavPathOptimization.Domain.Common.Errors;

public partial class Errors
{
    public static class Schedule
    {
        public static readonly Error UavModelMaxFlightTimeExceeded = Error.Failure(
            code: "UavModel.MaxFlightTimeExceeded",
            description: "Uav model max flight time exceeded."
        );
    }
}