using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UavPathOptimization.Infrastructure.Persistence.EntityFramework;

public class ApplicationDbContext : IdentityDbContext<InfrastructureUser, IdentityRole<Guid>, Guid>
{
    public DbSet<InfrastructureUser> Users { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}