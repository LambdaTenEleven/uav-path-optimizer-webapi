using ErrorOr;

namespace UavPathOptimization.Domain.Common.Errors;

public partial class Errors
{
    public static class Schedule
    {
        public static readonly Error UavModelMaxFlightTimeExceeded = Error.Failure(
            code: "Schedule.UavMaxFlightTimeExceeded",
            description: "Uav model max flight time exceeded."
        );

        public static readonly Error AbrasOperatingTimeExceeded = Error.Failure(
            code: "Abras.OperatingTimeExceeded",
            description: "Abras operating time exceeded."
        );
    }
}