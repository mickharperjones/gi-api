using Microsoft.AspNetCore.Mvc;
using EnvironmentService;

namespace GI.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EnvironmentController : ControllerBase {

    private readonly ILogger<EnvironmentController> logger;
    private readonly IEnvironmentService envService;

        public EnvironmentController(ILogger<EnvironmentController> logger, IEnvironmentService envService) {
        this.logger = logger;
        this.envService = envService;
    }

    [HttpGet("stationlatest/{stationId}")]
    public async Task<IActionResult> StationLatest(string stationId)
    {
        var reading = await envService.StationLatestReading(stationId);

        var valueMeasure = reading.items.measures.Where(m => m.latestReading != null).FirstOrDefault();

        var json = new { 
            Label = $"{reading.items.town}, {reading.items.label}",
            Latitude = reading.items.lat,
            Longitude = reading.items.longitude,
            Value = (valueMeasure != null) ? valueMeasure.latestReading.value : 0M,
            TakenAt = (valueMeasure != null) ? valueMeasure.latestReading.dateTime : DateTime.Now,
            Unit = (valueMeasure != null) ? valueMeasure.unitName : "m",
            Location = (valueMeasure != null) ? valueMeasure.label.Split(" - ").First() : "",
            Parameter = (valueMeasure != null) ? valueMeasure.parameterName : null
        };

        return Ok(json);
    }

    [HttpGet("stationreadings/{stationId}")]
    public async Task<IActionResult> StationReadings(string stationId)
    {
        var readings = await envService.StationReadings(stationId);

        var json = readings.items.Select (r => new {
            Value = r.value,
            TakenAt = r.dateTime
        }).ToList();

        return Ok(json);
    }

    [HttpGet("floodwarnings")]
    public async Task<IActionResult> FloodWarnings()
    {
        var latitude = 53.44335;
        var longitude = -1.94888;

        var results = await envService.FloodWarnings(latitude, longitude, 6);

        return Ok(results.items.ToList());
    }
}