using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UavPathOptimization.Application.Services;

namespace UavPathOptimization.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // services.AddMediatR(configuration =>
        //     configuration.RegisterServicesFromAssembly(assembly));
        services.AddScoped<IPathOptimizerService, PathOptimizerService>();

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}