using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Domain.Entities.Results;

public class UavPathSchedule
{
    public Guid UavModelId { get; set; }

    public IList<UavScheduleEntry> UavScheduleEntries { get; set; } = null!;
}