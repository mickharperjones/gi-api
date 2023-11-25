using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficService;

namespace Microsoft.Extensions.DependencyInjection;

public static class AppBuilder
{
    public static IServiceCollection AddTrafficService(
        this IServiceCollection services)
    {
        services.AddSingleton<IHighwaysEnglandService, HighwaysEnglandService>();

        return services;
    }
}
