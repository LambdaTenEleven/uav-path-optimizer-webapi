using ErrorOr;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UavPathOptimization.Application.Common.Behaviours;
using UavPathOptimization.Domain.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace UavPathOptimization.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager builderConfiguration)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddValidatorsFromAssembly(assembly);

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assembly);
        services.AddSingleton(config);

        services.AddScoped<IMapper, ServiceMapper>();

        // password settings
        var passwordSettings = new PasswordSettings();
        builderConfiguration.Bind(PasswordSettings.SectionName, passwordSettings);
        services.AddSingleton(Options.Create(passwordSettings));

        return services;
    }
}