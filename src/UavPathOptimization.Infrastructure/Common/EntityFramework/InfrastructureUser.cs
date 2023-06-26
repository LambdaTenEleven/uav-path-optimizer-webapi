using Microsoft.AspNetCore.Identity;

namespace UavPathOptimization.Infrastructure.Common.EntityFramework;

public class InfrastructureUser : IdentityUser<Guid>
{
    // TODO The infrastructure user should have the same fields as the domain user.
}