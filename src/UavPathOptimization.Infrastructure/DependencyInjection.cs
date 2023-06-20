using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UavPathOptimization.Application.Common.Authentication;
using UavPathOptimization.Application.Common.Persistence;
using UavPathOptimization.Application.Common.Services;
using UavPathOptimization.Domain.Common;
using UavPathOptimization.Domain.Common.Settings;
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
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(builderConfiguration.GetConnectionString("DefaultConnection"))
        );

        services.AddAuth(builderConfiguration);

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        // password settings
        var passwordSettings = new PasswordSettings();
        builderConfiguration.Bind(PasswordSettings.SectionName, passwordSettings);

        services.AddIdentityCore<InfrastructureUser>(options =>
        {
            // Configure Identity options
            options.Password.RequireDigit = passwordSettings.RequireDigit;
            options.Password.RequireLowercase = passwordSettings.RequireLowercase;
            options.Password.RequireUppercase = passwordSettings.RequireUppercase;
            options.Password.RequireNonAlphanumeric = passwordSettings.RequireNonAlphanumeric;
            options.Password.RequiredLength = passwordSettings.RequiredLength;
        })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<UserStore<InfrastructureUser, IdentityRole<Guid>, ApplicationDbContext, Guid>>()
            .AddRoleStore<RoleStore<IdentityRole<Guid>, ApplicationDbContext, Guid>>()
            .AddUserManager<UserManager<InfrastructureUser>>();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        var jwtSettings = new JwtSettings();
        builderConfiguration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            });

        return services;
    }
}