using Microsoft.AspNetCore.Identity;

namespace UavPathOptimization.Infrastructure.Persistence.EntityFramework;

public class InfrastructureUser : IdentityUser<Guid>
{
    // TODO The infrastructure user should have the same fields as the domain user.
}