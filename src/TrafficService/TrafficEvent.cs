using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TrafficService;

public enum EventCurrency
{
    Current = 1,
    Future = 2
}

/*
    * <item>
    <author>info@highwaysengland.co.uk</author>
    <category>Congestion</category>
    <category>Minor Disruption - up to 15 minutes delay</category>
    <description>Location : The M56 eastbound between junctions J3 and J2 . Reason : Congestion. Status : Currently Active. Return To Normal : Normal traffic conditions are expected between 10:30 and 10:45 on 14 December 2021. Delay : There are currently delays of 10 minutes against expected traffic. Earlier Reason : Caused by the earlier broken down vehicle. </description>
    <guid isPermaLink="false">GUID1-3608569</guid>
    <link>http://www.trafficengland.com/?evtID=3608569</link>
    <pubDate>Tue, 14 Dec 2021 10:37:20 GMT</pubDate>
    <title>M56 eastbound between J3 and J2 | Eastbound | Congestion</title>
    <reference>UF-21-12-14-200267</reference>
    <road>M56</road>
    <region>North West</region>
    <county>Manchester District</county>
    <latitude>53.39786</latitude>
    <longitude>-2.2640567</longitude>
    <overallStart>2021-12-14T09:13:16+00:00</overallStart>
    <overallEnd>2021-12-14T10:40:17+00:00</overallEnd>
    <eventStart>2021-12-14T09:13:16+00:00</eventStart>
    <eventEnd>2021-12-14T10:40:17+00:00</eventEnd>
    </item>
    */
public class TrafficEvent
{
    public List<string> Categories { get; set; }
    public List<Tuple<string,string>> DescriptiveItems { get; set; }
    public string ExternalUrl { get; set; }
    public string Title { get; set; }
    public string Road { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public EventCurrency CurrencyType { get; set; }
    public bool NoDelay { get; set; }

    public static TrafficEvent Create(XElement item)
    {
        TrafficEvent evt = new TrafficEvent();

        evt.Categories = item.Elements("category").Select(a => a.Value).ToList();

        evt.NoDelay = evt.Categories.Contains("No Delay");

        var desc = item.Element("description");

        string description = desc != null ? desc.Value : "";

        evt.DescriptiveItems = new List<Tuple<string, string>>();

        var parts = description.Split('.');
        foreach (string part in parts)
        {
            var bits = part.Split(':');

            if (bits.Length != 2)
                continue;

            evt.DescriptiveItems.Add(new Tuple<string, string>(bits[0].Trim(), bits[1].Trim()));
        }

        evt.ExternalUrl = item.Element("link") != null ? item.Element("link").Value : string.Empty;
        evt.Title = item.Element("title") != null ? item.Element("title").Value : string.Empty;
        evt.Road = item.Element("road") != null ? item.Element("road").Value : string.Empty;

        var lat = item.Element("latitude");
        if (lat != null)
            evt.Latitude = Double.Parse(lat.Value);

        var longi = item.Element("longitude");
        if (longi != null)
            evt.Longitude = Double.Parse(longi.Value);

        var start = item.Element("eventStart");
        if (start != null)
            evt.Start = DateTimeOffset.Parse(start.Value, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None).LocalDateTime;

        var end = item.Element("eventEnd");
        if (end != null)
            evt.End = DateTimeOffset.Parse(end.Value, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None).LocalDateTime;

        evt.CurrencyType = (evt.Start < DateTime.Now) ? EventCurrency.Current : EventCurrency.Future;

        return evt;
    }
}

