using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TrafficService;

public interface IHighwaysEnglandService
{
    Task<XDocument> GetFeed(FeedType feedType);
    List<TrafficEvent> AllEventsByRoad(XDocument source, string road, bool currentOnly = true);
    List<TrafficEvent> PriorityEventsByRoad(XDocument source, string road, bool currentOnly = true);
    List<TrafficEvent> PriorityEventsByRoads(XDocument source, string[] roads, bool currentOnly = true);
}
