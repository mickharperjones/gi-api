using NewsService;

namespace Microsoft.Extensions.DependencyInjection; 

public static class AppBuilderExtensions
{

    public static IServiceCollection AddNewsService(
            this IServiceCollection services)
    {
        services.AddSingleton<IRssNewsFeedReader, RssNewsFeedReader>();
        services.AddSingleton<INewsService, RssNewsService>();

        return services;
    }
}
