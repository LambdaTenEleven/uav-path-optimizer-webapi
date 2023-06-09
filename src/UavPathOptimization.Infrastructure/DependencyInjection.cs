using Microsoft.Extensions.DependencyInjection;
using UavPathOptimization.Application.Common.Authentication;
using UavPathOptimization.Application.Common.Services;
using UavPathOptimization.Infrastructure.Authentication;
using UavPathOptimization.Infrastructure.Services;

namespace UavPathOptimization.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}