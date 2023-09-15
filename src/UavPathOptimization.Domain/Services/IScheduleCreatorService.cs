using ErrorOr;
using UavPathOptimization.Domain.Contracts;
using UavPathOptimization.Domain.Entities.Schedule;
using UavPathOptimization.Domain.Entities.UavEntities;
using UnitsNet;

namespace UavPathOptimization.Domain.Services;

public interface IScheduleCreatorService
{
    ErrorOr<UavSchedule> CreateScheduleForUavPath(
        UavPathDto path,
        DateTime departureTimeStart,
        TimeSpan monitoringTime,
        TimeSpan chargingTime,
        UavModel uavModel);

    ErrorOr<AbrasSchedule> CreateScheduleForAbrasPath(
        IList<GeoCoordinateDto> optimizedPath,
        DateTime departureTimeStart,
        TimeSpan operatingTime,
        Speed abrasSpeed,
        GeoCoordinateDto abrasStartPoint);
}