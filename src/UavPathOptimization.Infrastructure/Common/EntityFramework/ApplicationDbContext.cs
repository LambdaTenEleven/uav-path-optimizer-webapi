using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Infrastructure.Common.EntityFramework;

public class ApplicationDbContext : IdentityDbContext<InfrastructureUser, IdentityRole<Guid>, Guid>
{
    public DbSet<InfrastructureUser> Users { get; set; } = null!;

    public DbSet<UavModel> UavModels { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}