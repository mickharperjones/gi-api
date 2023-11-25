using System;

namespace WeatherService;

public class WeatherForecastItem
{
    public DateTime Time { get; set; }
    public int SignificantWeatherCode { get; set; }
    public string SignificantWeather { get; set; }
    public double Temperature { get; set; }
    public double FeelsLikeTemperature { get; set; }
    public double WindSpeed10m { get; set; }
    public double WindGustSpeed10m { get; set; }
    public int UvIndex { get; set; }
    public double TotalPrecipAmount { get; set; }
    public int TotalSnowAmount { get; set; }
    public int PrecipitationProbability { get; set; }
    public string PrecipitationType { get; set; }
    public bool IsRainy { get; set; }
    public bool IsSnowy { get; set; }

    public static WeatherForecastItem Create(TimeSeriesItem item)
    {
        WeatherForecastItem forecast = new WeatherForecastItem() {
            Time = DateTimeOffset.Parse(item.time, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None).LocalDateTime,
            SignificantWeatherCode = item.significantWeatherCode,
            SignificantWeather = TranslateWeatherCode(item.significantWeatherCode),
            Temperature = item.screenTemperature,
            FeelsLikeTemperature = item.feelsLikeTemperature,
            WindSpeed10m = item.windSpeed10m,
            WindGustSpeed10m = item.windGustSpeed10m,
            UvIndex = item.uvIndex,
            TotalPrecipAmount = item.totalPrecipAmount,
            TotalSnowAmount = (int) item.totalSnowAmount,
            PrecipitationProbability = item.probOfPrecipitation,
            IsRainy = item.totalPrecipAmount > 0,
            IsSnowy = item.totalSnowAmount > 0,
            PrecipitationType = DeterminePrecipitationType(item.significantWeatherCode)
        };

        return forecast;
    }

    protected static string DeterminePrecipitationType (int significantWeatherCode)
    {
        string precipitationType = "Rain";

        switch (significantWeatherCode)
        {
            case 16:
            case 17:
            case 18: { precipitationType = "Sleet"; break; }
            case 19:
            case 20:
            case 21: { precipitationType = "Hail"; break; }
            case 22:
            case 23:
            case 24:
            case 25:
            case 26:
            case 27:
            case 28:
            case 29: { precipitationType = "Snow"; break; }
            default: { precipitationType = "Rain"; break; }
        }

        return precipitationType;
    }

    protected static string TranslateWeatherCode (int significantWeatherCode)
    {
        string code = "";

        switch (significantWeatherCode)
        {
            case 0: { code = "Clear night"; break; }
            case 1: { code = "Sunny day"; break; }
            case 2:
                {
                    code = "Partly cloudy"; break;
                }
            case 3:
                {
                    code = "Partly cloudy"; break;
                }
            case 4:
                {
                    code = "Not used"; break;
                }
            case 5:
                {
                    code = "Mist"; break;
                }
            case 6:
                {
                    code = "Fog"; break;
                }
            case 7:
                {
                    code = "Cloudy"; break;
                }
            case 8:
                {
                    code = "Overcast"; break;
                }
            case 9:
                {
                    code = "Light rain shower"; break;
                }
            case 10:
                {
                    code = "Light rain shower"; break;
                }
            case 11:
                {
                    code = "Drizzle"; break;
                }
            case 12:
                {
                    code = "Light rain"; break;
                }
            case 13:
                {
                    code = "Heavy rain shower"; break;
                }
            case 14:
                {
                    code = "Heavy rain shower"; break;
                }
            case 15:
                {
                    code = "Heavy rain"; break;
                }
            case 16:
                {
                    code = "Sleet shower"; break;
                }
            case 17:
                {
                    code = "Sleet shower"; break;
                }
            case 18:
                {
                    code = "Sleet"; break;
                }
            case 19:
                {
                    code = "Hail shower"; break;
                }
            case 20:
                {
                    code = "Hail shower"; break;
                }
            case 21:
                {
                    code = "Hail"; break;
                }
            case 22:
                {
                    code = "Light snow shower"; break;
                }
            case 23:
                {
                    code = "Light snow shower"; break;
                }
            case 24:
                {
                    code = "Light snow"; break;
                }
            case 25:
                {
                    code = "Heavy snow shower"; break;
                }
            case 26:
                {
                    code = "Heavy snow shower"; break;
                }
            case 27:
                {
                    code = "Heavy snow"; break;
                }
            case 28:
                {
                    code = "Thunder shower"; break;
                }
            case 29:
                {
                    code = "Thunder shower"; break;
                }
            case 30:
                {
                    code = "Thunder"; break;
                }
            default: { code = "Unknown"; break; }
        }

        return code;
    }
}
