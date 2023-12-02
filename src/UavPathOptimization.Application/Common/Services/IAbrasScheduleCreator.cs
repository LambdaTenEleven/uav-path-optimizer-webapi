using ErrorOr;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities.Results;
using UavPathOptimization.Domain.Entities.Schedule;

namespace UavPathOptimization.Application.Common.Services;

public interface IAbrasScheduleCreator
{
    public ErrorOr<UavScheduleResult> CreateScheduleForAbras(
        IList<UavSchedule> uavSchedules,
        double speed,
        GeoCoordinateDto abrasDepotLocation,
        DateTime departureTimeStart,
        TimeSpan chargingTime);
}