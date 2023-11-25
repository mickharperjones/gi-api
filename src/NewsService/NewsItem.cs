#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System;

namespace NewsService;

public class NewsItem
{
    public string ChannelName { get; set; }
    public string Copyright { get; set; }
    public string Title { get; set; }
    public string LinkUrl { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public string ThumbnailUrl { get; set; }

    public DateTime PublishedOn { get; set; }
    public string PublishedOnHtml { get; set; }
}
