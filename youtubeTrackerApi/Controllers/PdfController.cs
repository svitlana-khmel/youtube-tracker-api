// using Microsoft.AspNetCore.Mvc;
// using System.IO;
// using EyeTrackingApi.Models;
// using System.Linq;
// using System;
// using QuestPDF.Fluent;
// using QuestPDF.Helpers;
// using QuestPDF.Infrastructure;
// using System.Collections.Generic;

// // Models for the PDF data
// public class VideoAnalyticsData
// {
//     public string VideoTitle { get; set; } = "Advanced React Patterns - Complete Guide";
//     public string VideoUrl { get; set; } = "youtube.com/watch?v=abc123";
//     public string SessionTime { get; set; } = "May 28, 2025 13:15:00 UTC";
//     public int WatchDuration { get; set; } = 120;
//     public double EngagementRate { get; set; } = 72.5;
//     public int GazeEvents { get; set; } = 87;
//     public double AvgHeartRate { get; set; } = 74.2;
//     public int FullscreenUsage { get; set; } = 60;
//     public List<InteractionEvent> InteractionEvents { get; set; } = new List<InteractionEvent>();
//     public List<EngagementMetric> EngagementMetrics { get; set; } = new List<EngagementMetric>();
//     public List<string> BehaviorInsights { get; set; } = new List<string>();
//     public List<string> Recommendations { get; set; } = new List<string>();
// }

// public class InteractionEvent
// {
//     public string Type { get; set; }
//     public string Description { get; set; }
//     public string Time { get; set; }
// }

// public class EngagementMetric
// {
//     public string Name { get; set; }
//     public double Value { get; set; }
//     public string Unit { get; set; }
// }

// public class PdfRequest
// {
//     public VideoAnalyticsData AnalyticsData { get; set; }
// }

// [ApiController]
// [Route("api/pdf")]
// public class PdfController : ControllerBase
// {
//     [HttpPost("generate")]
//     public IActionResult GeneratePdf([FromBody] PdfRequest request)
//     {
//         var data = request?.AnalyticsData ?? GetSampleData();
//         var stream = new MemoryStream();

//         Document.Create(container =>
//         {
//             // First page
//             container.Page(page =>
//             {
//                 page.Size(PageSizes.A4);
//                 page.Margin(40);
//                 page.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken2));

//                 page.Header().Row(row =>
//                 {
//                     row.RelativeItem().Column(col =>
//                     {
//                         col.Item().Text("Video Engagement Analytics Report")
//                             .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
//                         col.Item().Text($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
//                             .FontSize(9).FontColor(Colors.Grey.Medium);
//                     });

//                     row.ConstantItem(100).Height(50).Placeholder();
//                 });

//                 page.Compose(context =>
//                 {
//                     context.Content().Column(col =>
//                     {
//                         // Video Info
//                         col.Item().PaddingVertical(10).Column(videoInfo =>
//                         {
//                             videoInfo.Item().Background(Colors.Grey.Lighten4).Padding(15).Column(info =>
//                             {
//                                 info.Item().Text("📹 Video Information").FontSize(14).Bold().FontColor(Colors.Blue.Darken1);
//                                 info.Item().PaddingTop(5).Row(row =>
//                                 {
//                                     row.RelativeItem().Text($"Title: {data.VideoTitle}").FontSize(10);
//                                 });
//                                 info.Item().Row(row =>
//                                 {
//                                     row.RelativeItem().Text($"URL: {data.VideoUrl}").FontSize(10);
//                                 });
//                                 info.Item().Row(row =>
//                                 {
//                                     row.RelativeItem().Text($"Session: {data.SessionTime}").FontSize(10);
//                                 });
//                             });
//                         });

//                         // Key Metrics
//                         col.Item().PaddingVertical(10).Column(metrics =>
//                         {
//                             metrics.Item().Text("📊 Key Metrics").FontSize(16).Bold().FontColor(Colors.Blue.Darken1);
//                             metrics.Item().PaddingTop(10).Row(row =>
//                             {
//                                 row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(15).Column(metric =>
//                                 {
//                                     metric.Item().Text("Watch Duration").FontSize(12).Bold();
//                                     metric.Item().Text($"{data.WatchDuration}s").FontSize(18).Bold().FontColor(Colors.Blue.Darken1);
//                                     metric.Item().Text("2 min session").FontSize(8).FontColor(Colors.Grey.Medium);
//                                 });

//                                 row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(15).Column(metric =>
//                                 {
//                                     metric.Item().Text("Engagement Rate").FontSize(12).Bold();
//                                     metric.Item().Text($"{data.EngagementRate}%").FontSize(18).Bold().FontColor(Colors.Green.Darken1);
//                                     metric.Item().Text($"{data.GazeEvents} gaze events").FontSize(8).FontColor(Colors.Grey.Medium);
//                                 });

//                                 row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(15).Column(metric =>
//                                 {
//                                     metric.Item().Text("Avg Heart Rate").FontSize(12).Bold();
//                                     metric.Item().Text($"{data.AvgHeartRate} BPM").FontSize(18).Bold().FontColor(Colors.Orange.Darken1);
//                                     metric.Item().Text("Physiological engagement").FontSize(8).FontColor(Colors.Grey.Medium);
//                                 });

//                                 row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(15).Column(metric =>
//                                 {
//                                     metric.Item().Text("Fullscreen Usage").FontSize(12).Bold();
//                                     metric.Item().Text($"{data.FullscreenUsage}s").FontSize(18).Bold().FontColor(Colors.Purple.Darken1);
//                                     metric.Item().Text("50% of session").FontSize(8).FontColor(Colors.Grey.Medium);
//                                 });
//                             });
//                         });

//                         // Engagement Distribution
//                         col.Item().PaddingVertical(10).Column(chart =>
//                         {
//                             chart.Item().Text("📈 Engagement Distribution").FontSize(16).Bold().FontColor(Colors.Blue.Darken1);
//                             chart.Item().PaddingTop(10).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(15).Column(chartContent =>
//                             {
//                                 chartContent.Item().Row(row =>
//                                 {
//                                     row.RelativeItem().Text("Highly Engaged").FontSize(10);
//                                     row.ConstantItem(100).Height(20).Background(Colors.Green.Lighten1).AlignCenter()
//                                         .Text("45%").FontSize(9).FontColor(Colors.White);
//                                 });
//                                 chartContent.Item().PaddingTop(5).Row(row =>
//                                 {
//                                     row.RelativeItem().Text("Moderately Engaged").FontSize(10);
//                                     row.ConstantItem(80).Height(20).Background(Colors.Yellow.Lighten1).AlignCenter()
//                                         .Text("30%").FontSize(9);
//                                 });
//                                 chartContent.Item().PaddingTop(5).Row(row =>
//                                 {
//                                     row.RelativeItem().Text("Low Engagement").FontSize(10);
//                                     row.ConstantItem(60).Height(20).Background(Colors.Orange.Lighten1).AlignCenter()
//                                         .Text("15%").FontSize(9);
//                                 });
//                                 chartContent.Item().PaddingTop(5).Row(row =>
//                                 {
//                                     row.RelativeItem().Text("Disengaged").FontSize(10);
//                                     row.ConstantItem(40).Height(20).Background(Colors.Red.Lighten1).AlignCenter()
//                                         .Text("10%").FontSize(9).FontColor(Colors.White);
//                                 });
//                             });
//                         });

//                         // Interaction Events
//                         col.Item().PaddingVertical(10).Column(events =>
//                         {
//                             events.Item().Text("🎯 Interaction Events").FontSize(16).Bold().FontColor(Colors.Blue.Darken1);
//                             events.Item().PaddingTop(10).Column(eventList =>
//                             {
//                                 foreach (var evt in data.InteractionEvents)
//                                 {
//                                     eventList.Item().PaddingBottom(5).Row(row =>
//                                     {
//                                         row.RelativeItem().Background(GetEventColor(evt.Type)).Padding(8).Row(eventRow =>
//                                         {
//                                             eventRow.RelativeItem().Text(evt.Description).FontSize(10);
//                                             eventRow.ConstantItem(80).Text(evt.Time).FontSize(9).FontColor(Colors.Grey.Medium);
//                                         });
//                                     });
//                                 }
//                             });
//                         });

//                         // Engagement Metrics
//                         col.Item().PaddingVertical(10).Column(patterns =>
//                         {
//                             patterns.Item().Text("📊 Engagement Patterns").FontSize(16).Bold().FontColor(Colors.Blue.Darken1);
//                             patterns.Item().PaddingTop(10).Column(patternList =>
//                             {
//                                 foreach (var metric in data.EngagementMetrics)
//                                 {
//                                     patternList.Item().PaddingBottom(8).Column(metricItem =>
//                                     {
//                                         metricItem.Item().Row(row =>
//                                         {
//                                             row.RelativeItem().Text(metric.Name).FontSize(11);
//                                             row.ConstantItem(60).Text($"{metric.Value}{metric.Unit}").FontSize(11).Bold();
//                                         });
//                                         metricItem.Item().PaddingTop(2).Row(row =>
//                                         {
//                                             row.RelativeItem().Height(8).Background(Colors.Grey.Lighten3).Row(progressRow =>
//                                             {
//                                                 progressRow.RelativeItem((float)metric.Value).Height(8).Background(Colors.Blue.Medium);
//                                                 progressRow.RelativeItem((float)(100 - metric.Value));
//                                             });
//                                         });
//                                     });
//                                 }
//                             });
//                         });
//                     });

//                     context.Footer().Row(row =>
//                     {
//                         row.RelativeItem().Text($"Page {context.PageNumber}").FontSize(9).FontColor(Colors.Grey.Medium);
//                         row.RelativeItem().AlignRight().Text("Video Analytics Report").FontSize(9).FontColor(Colors.Grey.Medium);
//                     });
//                 });
//             });

//             // Second page
//             container.Page(page =>
//             {
//                 page.Size(PageSizes.A4);
//                 page.Margin(40);
//                 page.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken2));

//                 page.Header().Text("Detailed Analysis & Recommendations").FontSize(18).Bold().FontColor(Colors.Blue.Darken2);

//                 page.Compose(context =>
//                 {
//                     context.Content().Column(col =>
//                     {
//                         col.Item().PaddingVertical(10).Column(insights =>
//                         {
//                             insights.Item().Text("🧠 Behavioral Insights").FontSize(16).Bold().FontColor(Colors.Blue.Darken1);
//                             insights.Item().PaddingTop(10).Column(insightList =>
//                             {
//                                 foreach (var insight in data.BehaviorInsights)
//                                 {
//                                     insightList.Item().PaddingBottom(5).Row(row =>
//                                     {
//                                         row.ConstantItem(10).Text("•").FontSize(12).FontColor(Colors.Blue.Medium);
//                                         row.RelativeItem().Text(insight).FontSize(11);
//                                     });
//                                 }
//                             });
//                         });

//                         col.Item().PaddingVertical(15).Column(recommendations =>
//                         {
//                             recommendations.Item().Text("💡 Recommendations").FontSize(16).Bold().FontColor(Colors.Green.Darken1);
//                             recommendations.Item().PaddingTop(10).Column(recList =>
//                             {
//                                 foreach (var rec in data.Recommendations)
//                                 {
//                                     recList.Item().PaddingBottom(5).Row(row =>
//                                     {
//                                         row.ConstantItem(10).Text("•").FontSize(12).FontColor(Colors.Green.Medium);
//                                         row.RelativeItem().Text(rec).FontSize(11);
//                                     });
//                                 }
//                             });
//                         });

//                         col.Item().PaddingVertical(15).Column(summary =>
//                         {
//                             summary.Item().Text("📋 Summary Statistics").FontSize(16).Bold().FontColor(Colors.Blue.Darken1);
//                             summary.Item().PaddingTop(10).Table(table =>
//                             {
//                                 table.ColumnsDefinition(columns =>
//                                 {
//                                     columns.RelativeColumn(2);
//                                     columns.RelativeColumn(1);
//                                     columns.RelativeColumn(2);
//                                 });

//                                 table.Header(header =>
//                                 {
//                                     header.Cell().Background(Colors.Blue.Lighten3).Padding(8).Text("Metric").FontSize(11).Bold();
//                                     header.Cell().Background(Colors.Blue.Lighten3).Padding(8).Text("Value").FontSize(11).Bold();
//                                     header.Cell().Background(Colors.Blue.Lighten3).Padding(8).Text("Interpretation").FontSize(11).Bold();
//                                 });

//                                 table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("Session Duration");
//                                 table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("2 minutes");
//                                 table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("Standard viewing session");

//                                 table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("Engagement Score");
//                                 table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("72.5%");
//                                 table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("High engagement level");

//                                 table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("Interaction Events");
//                                 table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("4 events");
//                                 table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("Active viewer behavior");

//                                 table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("Fullscreen Usage");
//                                 table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("50%");
//                                 table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text("Quality content indicator");
//                             });
//                         });
//                     });

//                     context.Footer().Row(row =>
//                     {
//                         row.RelativeItem().Text($"Page {context.PageNumber}").FontSize(9).FontColor(Colors.Grey.Medium);
//                         row.RelativeItem().AlignRight().Text("Video Analytics Report").FontSize(9).FontColor(Colors.Grey.Medium);
//                     });
//                 });
//             });

//         }).GeneratePdf(stream);

//         return File(stream.ToArray(), "application/pdf", "video-analytics-report.pdf");
//     }

//     private VideoAnalyticsData GetSampleData()
//     {
//         return new VideoAnalyticsData
//         {
//             VideoTitle = "Advanced React Patterns - Complete Guide",
//             VideoUrl = "youtube.com/watch?v=abc123",
//             SessionTime = "May 28, 2025 13:15:00 UTC",
//             WatchDuration = 120,
//             EngagementRate = 72.5,
//             GazeEvents = 87,
//             AvgHeartRate = 74.2,
//             FullscreenUsage = 60,
//             InteractionEvents = new List<InteractionEvent>
//             {
//                 new InteractionEvent { Type = "play", Description = "▶️ Video Started", Time = "13:15:00" },
//                 new InteractionEvent { Type = "fullscreen", Description = "⛶ Fullscreen Enabled", Time = "13:15:30" },
//                 new InteractionEvent { Type = "pause", Description = "⏸️ Video Paused", Time = "13:15:45" },
//                 new InteractionEvent { Type = "play", Description = "▶️ Playback Resumed", Time = "13:15:55" }
//             },
//             EngagementMetrics = new List<EngagementMetric>
//             {
//                 new EngagementMetric { Name = "Visual Attention", Value = 72, Unit = "%" },
//                 new EngagementMetric { Name = "Interaction Rate", Value = 45, Unit = "%" },
//                 new EngagementMetric { Name = "Retention Score", Value = 89, Unit = "%" }
//             },
//             BehaviorInsights = new List<string>
//             {
//                 "Strong initial engagement (70% gaze detection)",
//                 "One pause event detected (10s duration)",
//                 "Switched to fullscreen during middle section",
//                 "Consistent viewing pattern with minor scrubbing",
//                 "Focused, continuous viewer with minimal interruptions",
//                 "High engagement with physiological indicators",
//                 "Strategic pausing suggests active learning"
//             },
//             Recommendations = new List<string>
//             {
//                 "Content around 45-55s mark may need improvement",
//                 "High fullscreen usage indicates quality content",
//                 "Consider adding interactive elements for sustained attention",
//                 "Physiological data suggests good engagement levels",
//                 "Maintain current content quality standards",
//                 "Consider adding more interactive checkpoints"
//             }
//         };
//     }

//     private string GetEventColor(string eventType)
//     {
//         return eventType.ToLower() switch
//         {
//             "play" => Colors.Blue.Lighten4.ToString(),
//             "pause" => Colors.Yellow.Lighten4.ToString(),
//             "fullscreen" => Colors.Green.Lighten4.ToString(),
//             _ => Colors.Grey.Lighten4.ToString()
//         };
//     }
// }
