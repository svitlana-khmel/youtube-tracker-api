using EyeTrackingApi.Models;

public class TrackingDataPointDto
{
    public DateTime Timestamp { get; set; }
    public VideoState VideoState { get; set; } = new VideoState();
    public Biometrics Biometrics { get; set; } = new Biometrics();
    public GazeData? GazeData { get; set; }
}