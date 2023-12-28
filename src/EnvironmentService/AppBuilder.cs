using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvironmentService;

namespace Microsoft.Extensions.DependencyInjection;

public static class AppBuilder
{
    public static IServiceCollection AddEnvironmentService(
        this IServiceCollection services)
    {
        services.AddSingleton<IEnvironmentService, EnvironmentAgencyService>();

        return services;
    }
}
