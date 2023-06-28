using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Domain.Entities.Results;

public class UavScheduleResult
{
    public Guid UavModelId { get; set; }

    public IList<UavScheduleEntry> UavScheduleEntries { get; set; } = null!;
}