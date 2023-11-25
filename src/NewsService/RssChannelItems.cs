#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NewsService;

public class RssChannel
{
    public bool ResponseOk { get; set; }
    public string ChannelName { get; set; }
    public string Copyright { get; set; }
    public List<RssNewsItem> Items { get; set; }

    public RssChannel() {
        Items = new List<RssNewsItem>();
        ResponseOk = false;
    }

    public static RssChannel Create (XDocument feedDoc)
    {
        RssChannel feed = new RssChannel() { 
            ChannelName = "Unknown"
        };

        var channel = feedDoc.Descendants("channel").FirstOrDefault();

        if (channel == null)
        {
            return feed;
        }

        // channel name
        var name = channel.Element("title");
        if (name != null)
            feed.ChannelName =  name.Value;

        // copyright
        var copyright = channel.Element("copyright");
        if (copyright != null)
            feed.Copyright =  copyright.Value;

        feed.Items = channel.Elements("item")
            .Select(a => RssNewsItem.Create(a))
            .ToList();

        return feed;
    }
}

public class RssNewsItem
{
    public string Title { get; set; }
    public string LinkUrl { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public string ThumbnailUrl { get; set; }

    public DateTime PublishedOn { get; set; }

    protected RssNewsItem() { }

    public static RssNewsItem Create (XElement feeditem)
    {
        RssNewsItem item = new RssNewsItem();

        var title = feeditem.Element("title");
        if (title != null)
            item.Title = title.Value;

        var link = feeditem.Element("link");
        if (link != null)
            item.LinkUrl = link.Value;

        var description = feeditem.Element("description");
        if (description != null)
            item.Description =  description.Value;

        var author = feeditem.Element("author");
        if (author != null)
            item.Author =  author.Value;

        var thumbnail = feeditem.Element("{http://search.yahoo.com/mrss/}thumbnail");
        if (thumbnail != null)
        {
            var attr = thumbnail.Attribute("url");

            if (attr != null)
                item.ThumbnailUrl = attr.Value;
        }

        var pubDate = feeditem.Element("pubDate");
        if (pubDate != null)
            item.PublishedOn = DateTimeOffset.Parse(pubDate.Value, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None).LocalDateTime;
        else
            item.PublishedOn = DateTime.Today;

        return item;
    }
}
