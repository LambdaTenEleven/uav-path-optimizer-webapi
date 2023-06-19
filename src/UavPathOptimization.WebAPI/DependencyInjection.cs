using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UavPathOptimization.Infrastructure.Authentication;

namespace UavPathOptimization.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, ConfigurationManager builderConfiguration)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddControllers();

        return services;
    }
}