namespace UavPathOptimization.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        return services;
    }
}