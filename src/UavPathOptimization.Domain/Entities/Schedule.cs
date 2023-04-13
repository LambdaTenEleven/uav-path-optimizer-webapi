using UavPathOptimization.Domain.Entities.Base;

namespace UavPathOptimization.Domain.Entities;

public class Schedule : BaseEntity
{
    public Uav Uav { get; set; }

    public IEnumerable<ScheduleEntry> ScheduleEntries { get; set; } = new List<ScheduleEntry>();
}