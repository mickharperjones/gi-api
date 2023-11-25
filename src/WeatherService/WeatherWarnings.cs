using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace WeatherService;

public class WeatherWarnings
{
    public List<Warning> Warnings { get; set; }

    public bool AnyWarnings { 
        get {
            return Warnings == null ? false : Warnings.Any();
        }
    }

    public string Title { get; set; }
    public string Copyright { get; set; }

    public static WeatherWarnings Create (XDocument feed)
    {
        WeatherWarnings model = new WeatherWarnings() { Warnings = new List<Warning>() };

        var channel = feed.Descendants("channel").FirstOrDefault();

        if (channel == null)
            return model;

        model.Title = channel.Element("title").Value;
        model.Copyright = channel.Element("copyright").Value;

        // create warning items.
        model.Warnings = channel.Elements("item").Select(a => Warning.Create(a)).ToList();

        return model;
    }
}

public class Warning
{
    public string Title { get; set; }
    public string Link { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    public static Warning Create (XElement item)
    {
        Warning warning = new Warning();

        warning.Title = item.Element("title").Value;
        warning.Link = item.Element("link").Value;

        string desc = item.Element("description").Value;

        if (!string.IsNullOrEmpty(desc))
        {
            var parts = desc.Split(':');

            if (parts.Length > 1)
                warning.Description = parts[1].Trim();
        }

        var enclosure = item.Element("enclosure");

        if (enclosure != null)
        {
            warning.ImageUrl = enclosure.Attribute("url").Value;
        }

        return warning;
    }
}
