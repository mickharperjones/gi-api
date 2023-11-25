using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common;
using Microsoft.AspNetCore.WebUtilities;

namespace WeatherService;

public class MetOfficeWeatherService : IMetOfficeWeatherService
{
    private const string MO_API_BASE_URL = "https://rgw.5878-e94b1c46.eu-gb.apiconnect.appdomain.cloud";
    private const string MO_API_HOURLY_SPOT_DATA = "/metoffice/production/v0/forecasts/point/hourly";
    private readonly string MO_API_FEED_CACHE_KEY = "MO_API_FEED_";

    private const string API_KEY = "28733c954aa25399986c64ff17d60da3";
    private const string API_SECRET = "74f97ff76e15a3874c69a25a7731d44f";

    private const string MO_RSS_WARNINGS_BASE_URL = "https://www.metoffice.gov.uk/public/data/PWSCache/WarningsRSS/Region/";
    private const string MO_RSS_WARNINGS_NW_URL = "nw";
    private readonly string MO_RSS_FEED_NW_CACHE_KEY = "MO_RSS_FEED_NW";

    private ICacheService cacheService;

    public MetOfficeWeatherService (ICacheService cacheService) => this.cacheService = cacheService;

    public async Task<WeatherModel> GetFeedAsync(WeatherLocation location)
    {
        WeatherModel model = null;

        // try get from cache.
        string cacheKey = $"{MO_API_FEED_CACHE_KEY}{location.NameKey}";
        model = cacheService.Get<WeatherModel>(cacheKey) as WeatherModel;

        if (model != null)
            return model;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(MO_API_BASE_URL);
            client.DefaultRequestHeaders.Add("X-IBM-Client-Id", API_KEY);
            client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", API_SECRET);

            var query = new Dictionary<string, string>() { 
                ["excludeParameterMetadata"] = "true",
                ["includeLocationName"] = "true",
                ["latitude"] = location.Latitude.ToString(),
                ["longitude"] = location.Longitude.ToString()
            };

            HttpResponseMessage response = await client.GetAsync(QueryHelpers.AddQueryString(MO_API_HOURLY_SPOT_DATA, query));

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    model = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherModel>(content);
                    cacheService.Add(cacheKey, new TimeSpan(0, 15, 0), model);
                }
            }
        }

        return model;
    }

    public List<WeatherForecastItem> HourlyForecast (WeatherModel source, int nextHourCount = 8)
    {
        if (source == null)
            return new List<WeatherForecastItem>();

        return source.features.First().properties.timeSeries.Take(nextHourCount).Select(a => WeatherForecastItem.Create(a)).ToList();
    }

    public async Task<WeatherWarnings> GetWarningsAsync()
    {
        WeatherWarnings model = null;

        // try get from cache.
        model = cacheService.Get<WeatherWarnings>(MO_RSS_FEED_NW_CACHE_KEY);

        if (model != null)
            return model;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(MO_RSS_WARNINGS_BASE_URL);

            HttpResponseMessage response = await client.GetAsync(MO_RSS_WARNINGS_NW_URL);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    var feedXml = XDocument.Parse(content);

                    model = WeatherWarnings.Create(feedXml);

                    cacheService.Add(MO_RSS_FEED_NW_CACHE_KEY, new TimeSpan(0, 30, 0), model);
                }
            }
        }


        return model;

    }
}


public class WeatherLocation
{
    public string NameKey { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}
