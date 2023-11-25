using System;
using System.Collections.Generic;
using WeatherService;

namespace Microsoft.Extensions.DependencyInjection;

public static class AppBuilder
{
    public static IServiceCollection AddWeatherService(
    this IServiceCollection services)
    {
        services.AddSingleton<IMetOfficeWeatherService, MetOfficeWeatherService>();

        return services;
    }
}
