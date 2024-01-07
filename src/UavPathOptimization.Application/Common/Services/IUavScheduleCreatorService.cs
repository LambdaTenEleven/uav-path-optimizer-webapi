using ErrorOr;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities.Schedule;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.Common.Services;

public interface IUavScheduleCreatorService
{
    ErrorOr<UavSchedule> CreateScheduleForUavPath(
        UavPathDto path,
        DateTime departureTimeStart,
        TimeSpan monitoringTime,
        TimeSpan chargingTime,
        UavModel uavModel,
        bool useWeatherData);
}