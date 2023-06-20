namespace UavPathOptimization.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
}