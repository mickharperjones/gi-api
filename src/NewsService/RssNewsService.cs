using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsService;

public class RssNewsService : INewsService
{
    private IRssNewsFeedReader feedReader;

    public RssNewsService(IRssNewsFeedReader feedReader) => this.feedReader = feedReader;

    public async Task<List<NewsItem>> GetNewsFeed(int daysOld = 3)
    {
        RssNewsFeedOptions menOptions = new RssNewsFeedOptions() { 
            KeyName = "MEN",
            FeedBaseUrl = "https://www.manchestereveningnews.co.uk",
            FeedPath = "/all-about/glossop?service=rss"
        };

        // get men channel feed.
        var menFeed = await feedReader.GetFeedAsync(menOptions);

        RssNewsFeedOptions chronOptions = new RssNewsFeedOptions()
        {
            KeyName = "GlossopChronicle",
            FeedBaseUrl = "https://www.questmedianetwork.co.uk",
            FeedPath = "/news/glossop-chronicle/feed.xml"
        };

        // get chronicle channel feed.
        var chronFeed = await feedReader.GetFeedAsync(chronOptions);

        // merge feeds
        List<NewsItem> news = new List<NewsItem>();

        if (menFeed != null && menFeed.ResponseOk)
        {
            news.AddRange(menFeed.Items
                .Where(b => b.PublishedOn > DateTime.Now.AddDays(-(double)daysOld))
                .Select(a => new NewsItem() {
                    Author = a.Author,
                    ChannelName = menFeed.ChannelName,
                    Copyright = menFeed.Copyright,
                    Description = a.Description,
                    LinkUrl = a.LinkUrl,
                    PublishedOn = a.PublishedOn,
                    PublishedOnHtml = a.PublishedOn.ToShortDateString(),
                    ThumbnailUrl = a.ThumbnailUrl,
                    Title = a.Title
            }));
        }

        if (chronFeed != null && chronFeed.ResponseOk)
        {
            news.AddRange(chronFeed.Items
                .Where(b => b.PublishedOn > DateTime.Now.AddDays(-(double)daysOld))
                .Select(a => new NewsItem()
                {
                    Author = a.Author,
                    ChannelName = RefineChannelName(chronFeed.ChannelName),
                    Copyright = chronFeed.Copyright,
                    Description = a.Description,
                    LinkUrl = a.LinkUrl,
                    PublishedOn = a.PublishedOn,
                    PublishedOnHtml = a.PublishedOn.ToShortDateString(),
                    ThumbnailUrl = a.ThumbnailUrl,
                    Title = a.Title
                }));
        }

        return news;
    }
    protected string RefineChannelName (string input)
    {
        string name = input;

        if (input.IndexOf(':') >= 0)
        {
            string[] parts = input.Split(':');

            if (parts.Length > 0)
                name = parts[0].Trim();
        }

        return name;
    }    
}
