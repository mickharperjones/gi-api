using System.Collections.Generic;

namespace WeatherService;

public class Geometry
{
    public string type { get; set; }
    public List<double> coordinates { get; set; }
}

public class Location
{
    public string name { get; set; }
}

public class TimeSeriesItem
{
    public string time { get; set; }
    public double screenTemperature { get; set; }
    public double maxScreenAirTemp { get; set; }
    public double minScreenAirTemp { get; set; }
    public double screenDewPointTemperature { get; set; }
    public double feelsLikeTemperature { get; set; }
    public double windSpeed10m { get; set; }
    public int windDirectionFrom10m { get; set; }
    public double windGustSpeed10m { get; set; }
    public double max10mWindGust { get; set; }
    public int visibility { get; set; }
    public double screenRelativeHumidity { get; set; }
    public int mslp { get; set; }
    public int uvIndex { get; set; }
    public int significantWeatherCode { get; set; }
    public double precipitationRate { get; set; }
    public double totalPrecipAmount { get; set; }
    public double totalSnowAmount { get; set; }
    public int probOfPrecipitation { get; set; }
}

public class Properties
{
    public Location location { get; set; }
    public double requestPointDistance { get; set; }
    public string modelRunDate { get; set; }
    public List<TimeSeriesItem> timeSeries { get; set; }
}

public class Feature
{
    public string type { get; set; }
    public Geometry geometry { get; set; }
    public Properties properties { get; set; }
}

public class WeatherModel
{
    public string type { get; set; }
    public List<Feature> features { get; set; }
}
