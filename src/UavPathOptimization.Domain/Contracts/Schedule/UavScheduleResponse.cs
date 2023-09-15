using UavPathOptimization.Domain.Entities.Schedule;

namespace UavPathOptimization.Domain.Contracts.Schedule;

public record UavScheduleResponse(
    IList<UavSchedule> UavPathSchedules, AbrasSchedule AbrasSchedule
);