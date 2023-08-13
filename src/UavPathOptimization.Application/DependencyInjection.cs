using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UavPathOptimization.Application.Common.Behaviours;
using UavPathOptimization.Domain.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using UavPathOptimization.Application.Common.Services;

namespace UavPathOptimization.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager builderConfiguration)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehaviour<,>));

        services.AddValidatorsFromAssembly(assembly);

        // password settings
        var passwordSettings = new PasswordSettings();
        builderConfiguration.Bind(PasswordSettings.SectionName, passwordSettings);
        services.AddSingleton(Options.Create(passwordSettings));

        services.AddSingleton<IDistanceCalculator, DistanceCalculator>();

        return services;
    }
}