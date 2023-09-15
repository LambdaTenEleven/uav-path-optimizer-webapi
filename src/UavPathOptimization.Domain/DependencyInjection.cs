using Microsoft.Extensions.DependencyInjection;
using UavPathOptimization.Domain.Services;

namespace UavPathOptimization.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IScheduleCreatorService, ScheduleCreatorService>();
        services.AddScoped<IPathOptimizationService, PathOptimizationService>();

        return services;
    }
}