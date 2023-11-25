
namespace GI.Api.Models;

public class ForecastItem {

    public string Hour {get; set;}
    public string SignificantWeather  {get; set;}
    public double Temperature  {get; set;}
    public double FeelsLikeTemperature  {get; set;}
    public double WindSpeed10m   {get; set;}
    public double WindGustSpeed10m {get; set;}
    public int UvIndex {get; set;}
    public double TotalPrecipAmount {get; set;}
    public int TotalSnowAmount {get; set;}
    public int PrecipitationProbability {get; set;}
    public bool IsRainy { get; set; }
    public bool IsSnowy { get; set; }
    public string Icon {get; set;}
    public string PrecipitationType {get; set;}

    public ForecastItem() {
        Hour = "";
        SignificantWeather = "";
        Icon = "";
        PrecipitationType = "";
    }
}