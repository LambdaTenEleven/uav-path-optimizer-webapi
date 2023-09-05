using UavPathOptimization.Domain.Entities.Base;

namespace UavPathOptimization.Domain.Entities;

public class User : Entity
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
}