using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeatherService;

public interface IMetOfficeWeatherService
{
    Task<WeatherModel> GetFeedAsync(WeatherLocation location);
    List<WeatherForecastItem> HourlyForecast(WeatherModel source, int nextHourCount = 8);

    Task<WeatherWarnings> GetWarningsAsync();
}
