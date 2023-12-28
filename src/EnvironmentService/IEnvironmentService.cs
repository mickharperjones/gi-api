namespace EnvironmentService;

public interface IEnvironmentService
{
    Task<StationLatest> StationLatestReading (string stationId);
    Task<StationReadings> StationReadings (string stationId);
    Task<FloodResults> FloodWarnings (double latitude, double longitude, int radiusKm);
}
