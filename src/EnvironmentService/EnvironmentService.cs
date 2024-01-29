using System.Net.Http;
using System.Threading.Tasks;
using Common;

namespace EnvironmentService;

public class EnvironmentAgencyService: IEnvironmentService {

    private const string EA_BASE_URL = "http://environment.data.gov.uk";
    private const string STATION_READING_URL = "/flood-monitoring/id/stations";
    private const string FLOOD_WARNINGS_URL = "flood-monitoring/id/floods";

    private ICacheService cacheService;

    public EnvironmentAgencyService (ICacheService cacheService) => this.cacheService = cacheService;

    public async Task<StationLatest> StationLatestReading (string stationId) {
        string cacheKey = "EA_STATION_READING_LATEST_" + stationId;

        StationLatest latest;

        latest = cacheService.Get<StationLatest>(cacheKey) as StationLatest;

        if (latest != null)
            return latest;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(EA_BASE_URL);

            HttpResponseMessage response = await client.GetAsync($"{STATION_READING_URL}/{stationId}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    latest = Newtonsoft.Json.JsonConvert.DeserializeObject<StationLatest>(content);
                    cacheService.Add(cacheKey, new TimeSpan(0, 15, 0), latest);
                }
            }
        }

        return latest;
    }

    public async Task<StationReadings> StationReadings (string stationId) {
        string cacheKey = "EA_STATION_READINGS_" + stationId;

        StationReadings readings;

        readings = cacheService.Get<StationReadings>(cacheKey) as StationReadings;

        if (readings != null)
            return readings;

        var today = DateTime.Now;
        var threeDaysEarlier = today.AddDays(-3);
        var queryString = $"?startdate={threeDaysEarlier.ToString("yyyy-MM-dd")}&enddate={today.ToString("yyyy-MM-dd")}";

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(EA_BASE_URL);

            HttpResponseMessage response = await client.GetAsync($"{STATION_READING_URL}/{stationId}/readings{queryString}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    readings = Newtonsoft.Json.JsonConvert.DeserializeObject<StationReadings>(content);
                    cacheService.Add(cacheKey, new TimeSpan(0, 15, 0), readings);
                }
            }
        }

        return readings;
    }

    public async Task<FloodResults> FloodWarnings (double latitude, double longitude, int radiusKm) {
        string cacheKey = $"EA_FLOOD_WARNINGS_{latitude}_{longitude}_{radiusKm}";

        FloodResults results;

        results = cacheService.Get<FloodResults>(cacheKey) as FloodResults;

        if (results != null)
            return results;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(EA_BASE_URL);

            HttpResponseMessage response = await client.GetAsync($"{FLOOD_WARNINGS_URL}?lat={latitude}&long={longitude}&dist={radiusKm}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    results = Newtonsoft.Json.JsonConvert.DeserializeObject<FloodResults>(content);
                    cacheService.Add(cacheKey, new TimeSpan(0, 15, 0), results);
                }
            }
        }

        return results;
    }
}