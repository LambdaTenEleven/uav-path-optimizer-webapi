using UavPathOptimization.Domain.Entities.Base;

namespace UavPathOptimization.Domain.Entities.UavEntities;

public class UavModel : BaseEntity
{
    public UavModel(Guid id, string name, double maxSpeed, TimeSpan maxFlightTime)
    {
        Id = id;
        Name = name;
        MaxSpeed = maxSpeed;
        MaxFlightTime = maxFlightTime;
    }

    public string Name { get; set; } = null!;

    public double MaxSpeed { get; set; }

    public TimeSpan MaxFlightTime { get; set; }
}