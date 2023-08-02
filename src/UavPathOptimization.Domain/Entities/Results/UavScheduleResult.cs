using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Domain.Entities.Results;

public record UavScheduleResult(IList<UavPathSchedule> UavPathSchedules, IList<UavScheduleEntry> AbrasScheduleEntries);