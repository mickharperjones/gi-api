using Microsoft.AspNetCore.Mvc;
using NewsService;

namespace GI.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NewsController : ControllerBase {

    private readonly ILogger<NewsController> logger;
    private readonly INewsService newsService;

    public NewsController(ILogger<NewsController> logger, INewsService newsService) {
        this.logger = logger;
        this.newsService = newsService;
    }

    [HttpGet("latest/{days}")]
    public async Task<IActionResult> LatestNews(int days)
    {
        var news = await newsService.GetNewsFeed(days);

        return Ok(news);
    }
}