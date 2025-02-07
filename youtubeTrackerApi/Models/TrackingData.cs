namespace EyeTrackingApi.Models;
public class TrackingData
{
    public DateTime Timestamp { get; set; }
    public VideoState? VideoState { get; set; }
    public GazeData? GazeData { get; set; }
}

public class VideoState
{
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
    public double X { get; set; }
    public double Y { get; set; }
}