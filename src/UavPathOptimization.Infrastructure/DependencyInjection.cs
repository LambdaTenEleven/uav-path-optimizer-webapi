using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UavPathOptimization.Application.Common.Authentication;
using UavPathOptimization.Application.Common.Persistence;
using UavPathOptimization.Application.Common.Services;
using UavPathOptimization.Infrastructure.Authentication;
using UavPathOptimization.Infrastructure.Persistence;
using UavPathOptimization.Infrastructure.Persistence.EntityFramework;
using UavPathOptimization.Infrastructure.Services;

namespace UavPathOptimization.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(builderConfiguration.GetConnectionString("DefaultConnection"))
        );

        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.Configure<JwtSettings>(builderConfiguration.GetSection(JwtSettings.SectionName));

        services.AddIdentityCore<InfrastructureUser>(options =>
            {
                // Configure Identity options
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<UserStore<InfrastructureUser, IdentityRole<Guid>, ApplicationDbContext, Guid>>()
            .AddRoleStore<RoleStore<IdentityRole<Guid>, ApplicationDbContext, Guid>>()
            .AddUserManager<UserManager<InfrastructureUser>>();

        return services;
    }
}