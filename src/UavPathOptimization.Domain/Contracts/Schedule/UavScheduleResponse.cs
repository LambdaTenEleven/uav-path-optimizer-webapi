using UavPathOptimization.Domain.Entities.Results;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Domain.Contracts.Schedule;

public record UavScheduleResponse(
    IList<UavPathSchedule> UavPathSchedules, IList<UavScheduleEntry> AbrasScheduleEntries
);