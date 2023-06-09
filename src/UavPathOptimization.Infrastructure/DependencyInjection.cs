using Microsoft.Extensions.DependencyInjection;
using UavPathOptimization.Application.Common.Authentication;
using UavPathOptimization.Infrastructure.Authentication;

namespace UavPathOptimization.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}