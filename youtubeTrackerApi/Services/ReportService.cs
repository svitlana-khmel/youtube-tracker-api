using EyeTrackingApi.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace EyeTrackingApi.Services;
public class ReportService : IReportService
{
    public async Task<byte[]> GenerateReportAsync(EyeTrackingData data)
    {
        var analysis = AnalyzeData(data);

        using var document = new PdfDocument();
        var page = document.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont("Arial", 12);

        // Add report content
        gfx.DrawString($"Video Analysis Report: {data.VideoTitle}", font, XBrushes.Black,
            new XRect(50, 50, page.Width, page.Height), XStringFormats.TopLeft);

        // Add attention points
        var y = 100;
        // foreach (var point in analysis.AttentionPoints)
        // {
        //     gfx.DrawString($"High attention at {point.Timestamp:mm:ss}: {point.Description}",
        //         font, XBrushes.Black, 50, y);
        //     y += 20;
        // }

        using var stream = new MemoryStream();
        document.Save(stream);
        return stream.ToArray();
    }

    private object AnalyzeData(EyeTrackingData data)
    {
        throw new NotImplementedException();
    }

    // public AnalysisResult AnalyzeData(EyeTrackingData data)
    // {
    //     var result = new AnalysisResult();

    //     // Analyze gaze patterns
    //     for (int i = 1; i < data.TrackingData.Count; i++)
    //     {
    //         var current = data.TrackingData[i];
    //         var previous = data.TrackingData[i - 1];

    //         if (current.GazeData != null && previous.GazeData != null)
    //         {
    //             // Calculate gaze movement speed
    //             var distance = Math.Sqrt(
    //                 Math.Pow(current.GazeData.X - previous.GazeData.X, 2) +
    //                 Math.Pow(current.GazeData.Y - previous.GazeData.Y, 2));

    //             // Detect rapid eye movements (potential excitement)
    //             if (distance > 500)
    //             {
    //                 result.AttentionPoints.Add(new AttentionPoint
    //                 {
    //                     Timestamp = current.Timestamp,
    //                     Type = AttentionType.HighExcitement,
    //                     Description = "Rapid eye movement detected"
    //                 });
    //             }

    //             // Detect focused attention (minimal eye movement)
    //             if (distance < 50)
    //             {
    //                 result.AttentionPoints.Add(new AttentionPoint
    //                 {
    //                     Timestamp = current.Timestamp,
    //                     Type = AttentionType.FocusedAttention,
    //                     Description = "Sustained focus detected"
    //                 });
    //             }
    //         }
    //     }

    //     return result;
    // }
}