using ErrorOr;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities.Results;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Domain.Services;

public interface IUavScheduleCreator
{
    ErrorOr<UavPathSchedule> CreateScheduleForUavPath(UavPathDto path,
        DateTime departureTimeStart,
        TimeSpan monitoringTime,
        TimeSpan chargingTime,
        UavModel uavModel);
}