using Newtonsoft.Json;

namespace EnvironmentService;

public class StationLatest {

    public string stationReference { get; set;}
    public StationItem items { get; set; }
}

public class StationItem {
    public string town { get; set; }
    public string label { get; set; }
    public string riverName { get; set; }
    public double lat { get; set; }
    
    [JsonProperty("long")]
    public double longitude { get; set; }
    public Measure[] measures { get; set; }
}

public class Measure {
    public string label { get; set; }
    public string parameterName { get; set; }
    public string unitName { get; set; }
    public int period { get; set; }
    public Reading latestReading { get; set; }
}

public class Reading {
    public DateTime dateTime { get; set; }
    public decimal value { get; set; }
}

public class StationReadings {

    public List<Reading> items { get; set; }
}