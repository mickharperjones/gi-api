namespace EnvironmentService;

public class FloodResults{
    public List<FloodWarning> items { get; set; }
}

public class FloodWarning {
    public string description { get; set; }
    public string message { get; set; }
    public string severity { get; set; }
    public int severityLevel { get; set; }
    public DateTime timeRaised { get; set; }
    public DateTime timeSeverityRaised { get; set; }
}

public class FloodArea { 
    public string county { get; set; }
    public string polygon { get; set; }
    public string riverOrSea { get; set; }

}