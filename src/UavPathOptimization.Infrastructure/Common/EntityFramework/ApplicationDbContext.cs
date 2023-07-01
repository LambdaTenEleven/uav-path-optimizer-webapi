using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UavPathOptimization.Domain.Entities.UavEntities;
using UnitsNet;
using UnitsNet.Units;

namespace UavPathOptimization.Infrastructure.Common.EntityFramework;

public class ApplicationDbContext : IdentityDbContext<InfrastructureUser, IdentityRole<Guid>, Guid>
{
    public DbSet<InfrastructureUser> Users { get; set; } = null!;

    public DbSet<UavModel> UavModels { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UavModel>().Property(x => x.MaxSpeed)
            .HasConversion(
                v => v.As(SpeedUnit.KilometerPerHour),
                v => Speed.From(v, SpeedUnit.KilometerPerHour)
            );
    }
}