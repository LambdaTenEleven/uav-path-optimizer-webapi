namespace UavPathOptimization.Domain.Entities;

public class UavScheduleResult
{
    public Guid UavModelId { get; set; }

    public IList<UavScheduleEntry> UavScheduleEntries { get; set; } = null!;
}