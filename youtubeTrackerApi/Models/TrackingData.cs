using System.Text.Json.Serialization;

namespace EyeTrackingApi.Models;

public class TrackingData
{
    public DateTime Timestamp { get; set; }
    public VideoState? VideoState { get; set; }
    public GazeData? GazeData { get; set; }
}

public class VideoState
{
    public int Id { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsFullscreen { get; set; }
    public int CurrentTime { get; set; }
    public bool IsPaused { get; set; }
}

public class GazeData
{
    public int Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
}

public class Biometrics
{
    public int Id { get; set; }
    public double Gsr { get; set; }
    public double Bpm { get; set; }
    public double AvgBpm { get; set; }
}

public class TrackingDataPoint
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public VideoState VideoState { get; set; } = new VideoState();
    public GazeData? GazeData { get; set; }
    public Biometrics Biometrics { get; set; } = new Biometrics();

    public int SessionDataId { get; set; }

    public SessionData? SessionData { get; set; }
}

public class SessionData
{
    public int Id { get; set; }
    public string PageUrl { get; set; } = string.Empty;
    public string VideoTitle { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
    [JsonPropertyName("trackingData")]
    public ICollection<TrackingDataPoint> TrackingDataPoints { get; set; } = new List<TrackingDataPoint>();
}


