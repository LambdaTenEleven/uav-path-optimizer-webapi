using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Domain.Contracts.Schedule;

public record UavScheduleResponse(
    Guid UavModelId,
    IList<UavScheduleEntry> UavScheduleEntries
);