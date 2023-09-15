using UavPathOptimization.Domain.Entities.Schedule;

namespace UavPathOptimization.Domain.Entities.Results;

public record UavScheduleResult(IList<UavSchedule> UavPathSchedules, AbrasSchedule AbrasSchedule);