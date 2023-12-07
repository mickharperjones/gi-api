using GI.Api.Models;
using Microsoft.AspNetCore.Mvc;
using WeatherService;

namespace GI.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly ILogger<WeatherController> logger;
    private readonly IMetOfficeWeatherService weatherService;

    public WeatherController(ILogger<WeatherController> logger, IMetOfficeWeatherService weatherService) {
        this.logger = logger;
        this.weatherService = weatherService;
    }

    [HttpGet("forecast/hourly/{hours?}", Name = "HourlyForecast")]
    public async Task<IActionResult> Get(int hours = 8)
    {
        WeatherLocation location = new WeatherLocation() { 
            NameKey = "Glossop",
            Latitude = 53.44335,
            Longitude = -1.94888
        };

        var weatherModel = await weatherService.GetFeedAsync(location);

        var forecast = weatherService.HourlyForecast(weatherModel, hours);

        var forecastResponse = forecast.Select(a => new ForecastItem
        {
            Hour = a.Time.ToString("HH:mm"),
            SignificantWeather = a.SignificantWeather,
            Temperature = Math.Round(a.Temperature,0, MidpointRounding.AwayFromZero),
            FeelsLikeTemperature = Math.Round(a.FeelsLikeTemperature, 0, MidpointRounding.AwayFromZero),
            WindSpeed10m = Math.Round(ConvertToMph(a.WindSpeed10m), 0, MidpointRounding.AwayFromZero),
            WindGustSpeed10m = Math.Round(ConvertToMph(a.WindGustSpeed10m), 0, MidpointRounding.AwayFromZero),
            UvIndex = a.UvIndex,
            TotalPrecipAmount = a.TotalPrecipAmount,
            TotalSnowAmount = a.TotalSnowAmount,
            PrecipitationProbability = a.PrecipitationProbability,
            IsRainy = a.IsRainy,
            IsSnowy = a.IsSnowy,
            Icon = LookupWeatherSymbol(a.SignificantWeatherCode),
            PrecipitationType = a.PrecipitationType
        }).ToList();

        return Ok(forecastResponse);
    }

    [HttpGet("warnings")]
    public async Task<IActionResult> Warnings()
    {
        var warnings = await weatherService.GetWarningsAsync();

        return Ok(warnings);
    }

    protected string LookupWeatherSymbol(int significantWeatherCode)
    {
        string icon = "";

        switch (significantWeatherCode)
        {
            case 0: { icon = "24b.png"; break; }
            case 1: { icon = "24a.png"; break; }
            case 2:
                {
                    icon = "22b.png"; break;
                }
            case 3:
                {
                    icon = "22a.png"; break;
                }
            case 4:
                {
                    icon = ""; break;
                }
            case 5:
                {
                    icon = "17.png"; break;
                }
            case 6:
                {
                    icon = "16.png"; break;
                }
            case 7:
                {
                    icon = "23a.png"; break;
                }
            case 8:
                {
                    icon = "20.png"; break;
                }
            case 9:
                {
                    icon = "12.png"; break;
                }
            case 10:
                {
                    icon = "12.png"; break;
                }
            case 11:
                {
                    icon = "15.png"; break;
                }
            case 12:
                {
                    icon = "15.png"; break;
                }
            case 13:
                {
                    icon = "9.png"; break;
                }
            case 14:
                {
                    icon = "9.png"; break;
                }
            case 15:
                {
                    icon = "14.png"; break;
                }
            case 16:
                {
                    icon = "8.png"; break;
                }
            case 17:
                {
                    icon = "8.png"; break;
                }
            case 18:
                {
                    icon = "8.png"; break;
                }
            case 19:
                {
                    icon = "3.png"; break;
                }
            case 20:
                {
                    icon = "3.png"; break;
                }
            case 21:
                {
                    icon = "3.png"; break;
                }
            case 22:
                {
                    icon = "5.png"; break;
                }
            case 23:
                {
                    icon = "5.png"; break;
                }
            case 24:
                {
                    icon = "7.png"; break;
                }
            case 25:
                {
                    icon = "6.png"; break;
                }
            case 26:
                {
                    icon = "6.png"; break;
                }
            case 27:
                {
                    icon = "6.png"; break;
                }
            case 28:
                {
                    icon = "2.png"; break;
                }
            case 29:
                {
                    icon = "2.png"; break;
                }
            case 30:
                {
                    icon = "2.png"; break;
                }
            default: { icon = ""; break; }
        }

        return icon;

    }

    protected double ConvertToMph (double metresPerSec)
    {
        return 2.23694 * metresPerSec;
    }

}
