using UavPathOptimization.Domain.Services;
using UavPathOptimization.Infrastructure.Common.EntityFramework;
using UavPathOptimization.Infrastructure.Services.Weather;

namespace UavPathOptimization.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        services.AddControllers();
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>(name: "Database");

        services.AddHttpClient<IWeatherClient, OpenMeteoWeatherClient>(
            client =>
            {
                var baseAddress = builderConfiguration["WeatherApiSettings:Uri"];
                client.BaseAddress = new Uri(baseAddress!);
            });

        return services;
    }
}