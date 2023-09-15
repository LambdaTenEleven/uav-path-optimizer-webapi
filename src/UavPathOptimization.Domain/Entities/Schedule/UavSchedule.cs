namespace UavPathOptimization.Domain.Entities.Schedule;

public record UavSchedule(Guid UavModelId, IList<UavScheduleEntry> UavScheduleEntries);