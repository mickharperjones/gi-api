using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common;

namespace TrafficService;

public enum FeedType
{
    NorthWestFull = 0
}

public class HighwaysEnglandService : IHighwaysEnglandService
{
    private const string HE_RSS_BASE_URL = "https://m.highwaysengland.co.uk";
    private const string HE_RSS_NORTHWEST_FULL_URL = "/feeds/rss/AllEvents/North%20West.xml";
    private readonly string HE_RSS_FEED_CACHE_KEY = "HE_RSS_FEED_";

    private ICacheService cacheService;

    public HighwaysEnglandService (ICacheService cacheService) => this.cacheService = cacheService;

    public async Task<XDocument> GetFeed(FeedType feedType)
    {
        XDocument eventsXml = null;

        // try get from cache.
        string cacheKey = $"{HE_RSS_FEED_CACHE_KEY}{Enum.GetName(typeof(FeedType), feedType)}";
        eventsXml = cacheService.Get<XDocument>(cacheKey) as XDocument;

        if (eventsXml != null)
            return eventsXml;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(HE_RSS_BASE_URL);

            HttpResponseMessage response = await client.GetAsync(HE_RSS_NORTHWEST_FULL_URL);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    eventsXml = XDocument.Parse(content);
                    cacheService.Add(cacheKey, new TimeSpan(0, 15, 0), eventsXml);
                }
            }
        }

        return eventsXml;
    }

    public List<TrafficEvent> AllEventsByRoad (XDocument source, string road, bool currentOnly = true)
    {
        var items = source.Descendants("item")
            .Where(a => a.Element("road") != null && a.Element("road").Value.Trim().Equals(road))
            .Select(b => TrafficEvent.Create(b))
            .ToList();

        if (currentOnly)
        {
            items = items.Where(a => a.CurrencyType == EventCurrency.Current)
                .ToList();
        }

        return items;
    }

    public List<TrafficEvent> PriorityEventsByRoad(XDocument source, string road, bool currentOnly = true)
    {
        var items = source.Descendants("item")
            .Where(a => (a.Element("road") != null && a.Element("road").Value.Trim().Equals(road)) &&
            !(a.Elements("category").Any(b => b.Value.Equals("No Delay"))))
            .Select(b => TrafficEvent.Create(b))
            .ToList();

        if (currentOnly)
        {
            items = items.Where(a => a.CurrencyType == EventCurrency.Current)
                .ToList();
        }

        return items;
    }

    public List<TrafficEvent> PriorityEventsByRoads(XDocument source, string[] roads, bool currentOnly = true)
    {
        var items = source.Descendants("item")
            .Where(a => (a.Element("road") != null && roads.Contains(a.Element("road").Value.Trim())) &&
            !(a.Elements("category").Any(b => b.Value.Equals("No Delay"))))
            .Select(b => TrafficEvent.Create(b))
            .ToList();

        if (currentOnly)
        {
            items = items.Where(a => a.CurrencyType == EventCurrency.Current)
                .ToList();
        }

        return items;
    }
}
