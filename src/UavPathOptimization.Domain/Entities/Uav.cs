using UavPathOptimization.Domain.Entities.Base;

namespace UavPathOptimization.Domain.Entities;

public class Uav : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public double MaxSpeed { get; set; }

    public TimeSpan MaxFlightTime { get; set; }
}