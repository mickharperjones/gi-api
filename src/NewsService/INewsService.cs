using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsService;

public interface INewsService
{
    Task<List<NewsItem>> GetNewsFeed(int daysOld = 3);
}
