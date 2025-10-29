public class SessionExportDto
{
    public string PageUrl { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public List<TrackingDataPointDto> TrackingData { get; set; } = new List<TrackingDataPointDto>();
}