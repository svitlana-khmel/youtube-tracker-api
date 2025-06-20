namespace EyeTrackingApi.Models
{

    public class VideoAnalyticsRequest
    {
        public string VideoTitle { get; set; }
        public string VideoUrl { get; set; }
        public DateTime? SessionStart { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public bool GazeDataRecorded { get; set; }
        public bool FullscreenUsed { get; set; }
        public List<GazePoint> GazePoints { get; set; }
        public List<string> EngagementSummary { get; set; }
    }

    public class GazePoint
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}