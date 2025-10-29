// public class TrackingPointDto
// {
//     public DateTime Timestamp { get; set; }
//     public double? X { get; set; }
//     public double? Y { get; set; }
//     public double? Width { get; set; }
//     public double? Height { get; set; }
//     public double? Bpm { get; set; }
//     public double? Gsr { get; set; }
//     public double? AvgBpm { get; set; }
// }

using EyeTrackingApi.Models;

public class TrackingDataPointDto
{
    public DateTime Timestamp { get; set; }
    public VideoState VideoState { get; set; } = new VideoState();
    public Biometrics Biometrics { get; set; } = new Biometrics();
    public GazeData? GazeData { get; set; }
}