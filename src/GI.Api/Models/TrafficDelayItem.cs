namespace GI.Api.Models;

public class TrafficDelayItem {
    public string Road { get; set; }
    public string Category { get; set; }
    public string Delay { get; set; }
    public string Location { get; set; }
    public string Reason { get; set; }
    public string Closures { get; set; }
    public string ExternalUrl { get; set; }

    public TrafficDelayItem () {
        Road = "";
        Category = "";
        Delay = "";
        Location = "";
        Reason = "";
        Closures = "";
        ExternalUrl = "";
    }
}

public class TrafficDelayHeadline {
    public string Road { get; set; }
    public bool Delayed { get; set; }
    
    public TrafficDelayHeadline() {
        Road = "";
        Delayed = false;
    }
}