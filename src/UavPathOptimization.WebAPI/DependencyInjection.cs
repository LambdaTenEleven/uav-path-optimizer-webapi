using UavPathOptimization.Infrastructure.Common.EntityFramework;

namespace UavPathOptimization.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, ConfigurationManager builderConfiguration)
    {
        services.AddControllers();
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>(name: "Database");

        return services;
    }
}