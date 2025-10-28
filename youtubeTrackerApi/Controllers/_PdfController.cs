
// using iText.Kernel.Pdf;
// using iText.Layout;
// using iText.Layout.Element;
// using iText.Layout.Properties;
// using iText.Kernel.Colors;
// using iText.Kernel.Font;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using EyeTrackingApi.Models;
using iText.IO.Font.Constants;
using System.Linq;
using System;


using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


[ApiController]
[Route("api/pdf")]
public class _PdfController : ControllerBase
{
    [HttpPost("generate")]
    public IActionResult GeneratePdf([FromBody] PdfRequest request)
    {
        var stream = new MemoryStream();
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Content().Column(col =>
                {
                    col.Item().Text("Video Analytics Report").FontSize(20).Bold().AlignCenter();
                    col.Item().Text(request.Text).FontSize(12);
                });
            });
        }).GeneratePdf(stream);

        return File(stream.ToArray(), "application/pdf", "report.pdf");
    }


    // [HttpPost("generate-analytics")]
    // public IActionResult GenerateAnalyticsReport([FromBody] VideoAnalyticsRequest request)
    // {
    //     if (request == null)
    //     {
    //         return BadRequest("Invalid request");
    //     }

    //     try
    //     {
    //         using (var stream = new MemoryStream())
    //         {
    //             using var writer = new PdfWriter(stream);
    //             using var pdf = new PdfDocument(writer);
    //             using var document = new Document(pdf);

    //             document.SetMargins(50, 50, 50, 50);

    //             var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
    //             var regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

    //             var title = new Paragraph("Video Analytics Report")
    //                 .SetFontSize(20)
    //                 .SetFont(boldFont)
    //                 .SetTextAlignment(TextAlignment.CENTER)
    //                 .SetMarginBottom(30);
    //             document.Add(title);

    //             AddDynamicAnalyticsContent(document, request, boldFont, regularFont);

    //             var footer = new Paragraph($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
    //                 .SetFontSize(10)
    //                 .SetFont(regularFont)
    //                 .SetTextAlignment(TextAlignment.CENTER)
    //                 .SetMarginTop(30)
    //                 .SetFontColor(ColorConstants.GRAY);
    //             document.Add(footer);

    //             byte[] pdfBytes = stream.ToArray();
    //             return File(pdfBytes, "application/pdf", $"analytics-report-{DateTime.Now:yyyyMMdd-HHmmss}.pdf");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"PDF generation failed: {ex.Message}");
    //         return StatusCode(500, "Failed to generate PDF");
    //     }
    // }

    // private void AddVideoAnalyticsContent(Document document, PdfFont boldFont, PdfFont regularFont)
    // {
    //     AddSectionHeader(document, "📹 Video Information", boldFont);
    //     AddInfoLine(document, "Video Watched:", "Mentality MVP presentation", boldFont, regularFont);
    //     AddInfoLine(document, "URL:", "https://www.youtube.com/watch?v=vk3aYt7tLXY", boldFont, regularFont);
    //     AddInfoLine(document, "Session Start:", "2025-05-28T13:15:04Z", boldFont, regularFont);
    //     AddInfoLine(document, "Playback Range:", "12s → 17s", boldFont, regularFont);

    //     document.Add(new Paragraph("\n"));

    //     AddSectionHeader(document, "🧠 Tracking Data", boldFont);
    //     AddInfoLine(document, "Gaze Data Recorded:", "Yes (2 points)", boldFont, regularFont);
    //     AddInfoLine(document, "Fullscreen Used:", "No", boldFont, regularFont);

    //     document.Add(new Paragraph("\n"));

    //     AddSectionHeader(document, "👁️ Viewer Looked At:", boldFont);
    //     var gazeList = new List().SetMarginLeft(20);
    //     gazeList.Add(new ListItem("(500, 500)"));
    //     gazeList.Add(new ListItem("(498, 507)"));
    //     document.Add(gazeList);

    //     document.Add(new Paragraph("\n"));

    //     AddSectionHeader(document, "📈 Engagement Summary", boldFont);
    //     var engagementList = new List().SetMarginLeft(20);
    //     engagementList.Add(new ListItem("Short viewing session (~5s)"));
    //     engagementList.Add(new ListItem("No pause or seek detected"));
    //     engagementList.Add(new ListItem("Viewer gaze detected, suggesting attention"));
    //     document.Add(engagementList);
    // }

    // private void AddDynamicAnalyticsContent(Document document, VideoAnalyticsRequest request, PdfFont boldFont, PdfFont regularFont)
    // {
    //     AddSectionHeader(document, "📹 Video Information", boldFont);
    //     AddInfoLine(document, "Video Watched:", request.VideoTitle ?? "N/A", boldFont, regularFont);
    //     AddInfoLine(document, "URL:", request.VideoUrl ?? "N/A", boldFont, regularFont);
    //     AddInfoLine(document, "Session Start:", request.SessionStart?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A", boldFont, regularFont);
    //     AddInfoLine(document, "Playback Range:", $"{request.StartTime}s → {request.EndTime}s", boldFont, regularFont);

    //     document.Add(new Paragraph("\n"));

    //     AddSectionHeader(document, "🧠 Tracking Data", boldFont);
    //     AddInfoLine(document, "Gaze Data Recorded:", request.GazeDataRecorded ? $"Yes ({request.GazePoints?.Count ?? 0} points)" : "No", boldFont, regularFont);
    //     AddInfoLine(document, "Fullscreen Used:", request.FullscreenUsed ? "Yes" : "No", boldFont, regularFont);

    //     document.Add(new Paragraph("\n"));

    //     if (request.GazePoints?.Any() == true)
    //     {
    //         AddSectionHeader(document, "👁️ Viewer Looked At:", boldFont);
    //         var gazeList = new List().SetMarginLeft(20);
    //         foreach (var point in request.GazePoints)
    //         {
    //             gazeList.Add(new ListItem($"({point.X}, {point.Y})"));
    //         }
    //         document.Add(gazeList);
    //         document.Add(new Paragraph("\n"));
    //     }

    //     AddSectionHeader(document, "📈 Engagement Summary", boldFont);
    //     var engagementList = new List().SetMarginLeft(20);
    //     if (request.EngagementSummary?.Any() == true)
    //     {
    //         foreach (var summary in request.EngagementSummary)
    //         {
    //             engagementList.Add(new ListItem(summary));
    //         }
    //     }
    //     else
    //     {
    //         engagementList.Add(new ListItem("No engagement data available"));
    //     }

    //     document.Add(engagementList);
    // }

    // private void AddSectionHeader(Document document, string headerText, PdfFont boldFont)
    // {
    //     var header = new Paragraph(headerText)
    //         .SetFontSize(14)
    //         .SetFont(boldFont)
    //         .SetMarginBottom(10)
    //         .SetFontColor(ColorConstants.DARK_GRAY);
    //     document.Add(header);
    // }

    // private void AddInfoLine(Document document, string label, string value, PdfFont boldFont, PdfFont regularFont)
    // {


    //     var paragraph = new Paragraph().SetMarginBottom(5);
    //     paragraph.Add(new Text(label + " ").SetFont(boldFont).SetFontColor(ColorConstants.BLACK));
    //     paragraph.Add(new Text(value).SetFont(regularFont).SetFontColor(ColorConstants.DARK_GRAY));
    //     document.Add(paragraph);
    // }
}


// using iText.Kernel.Pdf;
// using iText.Layout;
// using iText.Layout.Element;
// using Microsoft.AspNetCore.Mvc;
// using System.IO;
// using EyeTrackingApi.Models;


// EXAMPLE

