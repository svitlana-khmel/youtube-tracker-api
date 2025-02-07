namespace EyeTrackingApi.Models;
public class EyeTrackingData
{
    public string? PageUrl { get; set; }
    public string? VideoTitle { get; set; }
    public DateTime StartTime { get; set; }
    public List<TrackingData>? TrackingData { get; set; }
}
