using GI.Api.Models;
using Microsoft.AspNetCore.Mvc;
using TrafficService;

namespace GI.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TrafficController : ControllerBase
{
    private readonly ILogger<TrafficController> logger;
    private readonly IHighwaysEnglandService trafficService;

    public TrafficController(ILogger<TrafficController> logger, IHighwaysEnglandService trafficService) {
        this.logger = logger;
        this.trafficService = trafficService;
    }


    [HttpGet("delay/{road}")]
    public async Task<IActionResult> Get(string road) {

        if (!new string[] { "A57", "A628", "M60", "M56", "M6", "M62", "M67" }.Contains(road))
        {
            return BadRequest("Road is not supported. Must be one of: \"A57\", \"A628\", \"M60\", \"M56\", \"M6\", \"M62\", \"M67\"");
        }

        var trafficFeed = await trafficService.GetFeed(FeedType.NorthWestFull);

        var events = trafficService.PriorityEventsByRoad(trafficFeed, road, true);

        var roadTrafficData = events.Select(a => new TrafficDelayItem {
            Road = a.Road,
            Category = (a.Categories != null && a.Categories.Count > 0) ? a.Categories[0] : "Unknown",
            Delay = (a.Categories != null && a.Categories.Count > 1) ? a.Categories[1] : "Unknown",
            Location = a.DescriptiveItems.Any(b => b.Item1 == "Location") ? a.DescriptiveItems.Where(c => c.Item1 == "Location").First().Item2 : "Unknown",
            Reason = a.DescriptiveItems.Any(b => b.Item1 == "Reason") ? a.DescriptiveItems.Where(c => c.Item1 == "Reason").First().Item2 : "No reason given",
            Closures = a.DescriptiveItems.Any(b => b.Item1 == "Lane Closures") ? a.DescriptiveItems.Where(c => c.Item1 == "Lane Closures").First().Item2 : "No closures given",
            ExternalUrl = a.ExternalUrl
        });

        return Ok(roadTrafficData);
    }

    [HttpGet("delays")]
    public async Task<IActionResult> RoadPriority([FromQuery] string[] roads)
    {
        string[] checkList = new string[] { "A57", "A628", "M60", "M56", "M6", "M62", "M67" };

        if (roads.Any(a => !checkList.Contains(a)))
        {
            return BadRequest("Road is not supported. Must be one of: \"A57\", \"A628\", \"M60\", \"M56\", \"M6\", \"M62\", \"M67\"");
        }

        var trafficFeed = await trafficService.GetFeed(FeedType.NorthWestFull);

        // filter priority events by road.
        var events = trafficService.PriorityEventsByRoads(trafficFeed, roads, true);

   
        var roadTrafficData = events.Select(a => new TrafficDelayItem {
            Road = a.Road,
            Category = (a.Categories != null && a.Categories.Count > 0) ? a.Categories[0] : "Unknown",
            Delay = (a.Categories != null && a.Categories.Count > 1) ? a.Categories[1] : "Unknown",
            Location = a.DescriptiveItems.Any(b => b.Item1 == "Location") ? a.DescriptiveItems.Where(c => c.Item1 == "Location").First().Item2 : "Unknown",
            Reason = a.DescriptiveItems.Any(b => b.Item1 == "Reason") ? a.DescriptiveItems.Where(c => c.Item1 == "Reason").First().Item2 : "No reason given",
            Closures = a.DescriptiveItems.Any(b => b.Item1 == "Lane Closures") ? a.DescriptiveItems.Where(c => c.Item1 == "Lane Closures").First().Item2 : "No closures given",
            ExternalUrl = a.ExternalUrl
        });

        // de-dupe by location and delay.
        roadTrafficData = roadTrafficData
            .GroupBy(t => new { 
                Location = t.Location,
                Delay = t.Delay })
            .Select(g => g.First())
            .ToList();
   
        return Ok(roadTrafficData);
    }

    [HttpGet("delays/headline")]
    public async Task<IActionResult> RoadPriorityHeadline([FromQuery] string[] roads)
    {
        string[] checkList = new string[] { "A57", "A628", "M60", "M56", "M6", "M62", "M67" };

        if (roads.Any(a => !checkList.Contains(a)))
        {
            return BadRequest("Road is not supported. Must be one of: \"A57\", \"A628\", \"M60\", \"M56\", \"M6\", \"M62\", \"M67\"");
        }

        var trafficFeed = await trafficService.GetFeed(FeedType.NorthWestFull);

        // filter priority events by road.
        var events = trafficService.PriorityEventsByRoads(trafficFeed, roads, true);

        var roadTrafficData = roads.Select(a => new TrafficDelayHeadline { 
            Road = a,
            Delayed = events.Any(b => b.Road == a && !b.NoDelay)
        });

        return Ok(roadTrafficData);
    }

    [HttpGet("events/{road}")]
    public async Task<IActionResult> RoadEvents(string road)
    {
        if (!new string[] { "A57", "A628", "M60", "M56", "M6", "M62", "M67" }.Contains(road))
        {
            return BadRequest("Road is not supported. Must be one of: \"A57\", \"A628\", \"M60\", \"M56\", \"M6\", \"M62\", \"M67\"");
        }

        var trafficFeed = await trafficService.GetFeed(FeedType.NorthWestFull);

        // filter priority events by road.
        var events = trafficService.AllEventsByRoad(trafficFeed, road, true);

        var roadTrafficData = events.Select(a => new TrafficDelayItem {
            Road = a.Road,
            Category = (a.Categories != null && a.Categories.Count > 0) ? a.Categories[0] : "Unknown",
            Delay = (a.Categories != null && a.Categories.Count > 1) ? a.Categories[1] : "Unknown",
            Location = a.DescriptiveItems.Any(b => b.Item1 == "Location") ? a.DescriptiveItems.Where(c => c.Item1 == "Location").First().Item2 : "Unknown",
            Reason = a.DescriptiveItems.Any(b => b.Item1 == "Reason") ? a.DescriptiveItems.Where(c => c.Item1 == "Reason").First().Item2 : "No reason given",
            Closures = a.DescriptiveItems.Any(b => b.Item1 == "Lane Closures") ? a.DescriptiveItems.Where(c => c.Item1 == "Lane Closures").First().Item2 : "No closures given",
            ExternalUrl = a.ExternalUrl
        });

        return Ok(roadTrafficData);
    }
}