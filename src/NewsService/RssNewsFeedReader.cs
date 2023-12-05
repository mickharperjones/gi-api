using Common;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NewsService;

public interface IRssNewsFeedReader
{
    Task<RssChannel?> GetFeedAsync(RssNewsFeedOptions rssOptions);
}

public class RssNewsFeedReader : IRssNewsFeedReader
{
    private ICacheService cacheService;

    private readonly string RSS_FEED_CACHE_KEY = "RSS_FEED_";

    public RssNewsFeedReader (ICacheService cacheService) => this.cacheService = cacheService;

    public async Task<RssChannel?> GetFeedAsync(RssNewsFeedOptions rssOptions)
    {
        // try get from cache.
        string cacheKey = $"{RSS_FEED_CACHE_KEY}{rssOptions.KeyName}";
        var channel = cacheService.Get<RssChannel>(cacheKey);

        if (channel != null)
            return channel;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(rssOptions.FeedBaseUrl);

            HttpResponseMessage response = await client.GetAsync(rssOptions.FeedPath);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    XDocument feedDoc = XDocument.Parse(content);
                    channel = RssChannel.Create(feedDoc);
                    cacheService.Add(cacheKey, new TimeSpan(0, 60, 0), channel);

                    channel.ResponseOk = true;
                }
            }
        }

        return channel;
    }
}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class RssNewsFeedOptions
{ 
    public string KeyName { get; set; }
    public string FeedBaseUrl { get; set; }
    public string FeedPath { get; set; }
}
