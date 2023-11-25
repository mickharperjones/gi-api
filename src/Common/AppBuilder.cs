using Common;

namespace Microsoft.Extensions.DependencyInjection;
public static class AppBuilderExtensions
{

    public static IServiceCollection AddCommonServices(
            this IServiceCollection services)
    {
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}
