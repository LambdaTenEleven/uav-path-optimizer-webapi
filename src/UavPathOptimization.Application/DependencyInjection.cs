using ErrorOr;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UavPathOptimization.Application.Common.Behaviours;
using UavPathOptimization.Application.UseCases.Authentication.Commands;
using UavPathOptimization.Application.UseCases.Authentication.Commands.Register;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

        services.AddPipelineBehaviours();

        services.AddValidatorsFromAssembly(assembly);

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assembly);
        services.AddSingleton(config);

        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

    private static IServiceCollection AddPipelineBehaviours(this IServiceCollection services)
    {
        services.AddScoped<IPipelineBehavior<RegisterCommand, ErrorOr<AuthenticationResult>>,
            ValidateRegisterCommandBehaviour>();

        return services;
    }
}