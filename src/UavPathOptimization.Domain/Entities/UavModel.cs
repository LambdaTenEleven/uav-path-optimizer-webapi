using UavPathOptimization.Domain.Entities.Base;

namespace UavPathOptimization.Domain.Entities;

public class UavModel : BaseEntity
{
    public string Name { get; set; } = null!;

    public double MaxSpeed { get; set; }

    public TimeSpan MaxFlightTime { get; set; }
}