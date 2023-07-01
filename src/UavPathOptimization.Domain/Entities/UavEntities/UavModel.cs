using UavPathOptimization.Domain.Entities.Base;
using UnitsNet;

namespace UavPathOptimization.Domain.Entities.UavEntities;

public class UavModel : BaseEntity
{
    public string Name { get; set; } = null!;

    public Speed MaxSpeed { get; set; }

    public TimeSpan MaxFlightTime { get; set; }
}