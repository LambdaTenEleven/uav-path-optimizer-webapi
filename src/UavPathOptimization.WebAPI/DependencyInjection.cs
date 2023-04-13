﻿using FluentValidation;
using UavPathOptimization.Application.Services;

namespace UavPathOptimization.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        return services;
    }
}