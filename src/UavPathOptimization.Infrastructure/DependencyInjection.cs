using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UavPathOptimization.Application.Common.Authentication;
using UavPathOptimization.Application.Common.Persistance;
using UavPathOptimization.Application.Common.Services;
using UavPathOptimization.Infrastructure.Authentication;
using UavPathOptimization.Infrastructure.Persistance;
using UavPathOptimization.Infrastructure.Services;

namespace UavPathOptimization.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IUserRepository, InMemoryUserRepository>();

        services.Configure<JwtSettings>(builderConfiguration.GetSection(JwtSettings.SectionName));

        return services;
    }
}