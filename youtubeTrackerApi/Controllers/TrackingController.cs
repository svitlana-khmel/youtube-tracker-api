using EyeTrackingApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using EyeTrackingApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace EyeTrackingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TrackingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrackingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // public IActionResult Save([FromBody] SessionData sessionData)
        // {
        //     if (sessionData == null)
        //     {
        //         return BadRequest(new { ok = false, error = "No session data provided" });
        //     }

        //     // For now, just log it (replace with DB save later)
        //     Console.WriteLine("Received session data for video: " + sessionData.VideoTitle);
        //     Console.WriteLine($"Tracking points: {sessionData.TrackingData.Count}");

        //     // Example: store sessionData in database here

        //     return Ok(new { ok = true, message = "Session data saved successfully" });
        // }
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SessionData sessionData)
        {
            if (sessionData == null || sessionData.TrackingDataPoints.Count == 0)
                return BadRequest(new { ok = false, error = "No session data provided" });

            // Attach user from JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
                sessionData.UserId = userId;

            //sessionData.StartTime = DateTime.Parse(sessionData.StartTime);

            // Fix nested objects IDs
            // foreach (var point in sessionData.TrackingDataPoints)
            // {
            //     point.Timestamp = DateTime.Parse(point.Timestamp);
            // }

            await _context.Sessions.AddAsync(sessionData);
            await _context.SaveChangesAsync();

            return Ok(new { ok = true, message = "Session saved successfully" });
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessions = await _context.Sessions
                .Where(s => s.UserId == userId)
                .Select(s => new { s.Id, Date = s.StartTime.Date, Time = s.StartTime.TimeOfDay, s.PageUrl })
                .ToListAsync();

            return Ok(sessions);
        }
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id, [FromQuery] string format = "json")
        {
            try
            {
                var session = await _context.Sessions
                    .Include(s => s.TrackingDataPoints)
                        .ThenInclude(t => t.VideoState)
                    .Include(s => s.TrackingDataPoints)
                        .ThenInclude(t => t.Biometrics)
                    .Include(s => s.TrackingDataPoints)
                        .ThenInclude(t => t.GazeData)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (session == null) return NotFound();

                byte[] fileBytes;
                string contentType;
                string fileName = $"session-{id}.{format}";

                switch (format.ToLower())
                {
                    case "json":
                        var exportDto = new SessionExportDto
                        {
                            PageUrl = session.PageUrl,
                            StartTime = session.StartTime,
                            TrackingData = session.TrackingDataPoints
                                .Select(t => new TrackingDataPointDto
                                {
                                    Timestamp = t.Timestamp,
                                    VideoState = t.VideoState,
                                    Biometrics = t.Biometrics,
                                    GazeData = t.GazeData
                                }).ToList()
                        };

                        fileBytes = Encoding.UTF8.GetBytes(
                            JsonSerializer.Serialize(exportDto, new JsonSerializerOptions
                            {
                                WriteIndented = true,
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                            })
                        );
                        contentType = "application/json";
                        break;

                    case "txt":
                        // ✅ Dump entire structure for text format as well
                        var fullText = new StringBuilder();
                        fullText.AppendLine($"Page URL: {session.PageUrl}");
                        fullText.AppendLine($"Start Time: {session.StartTime}");
                        fullText.AppendLine($"Video Title: {session.VideoTitle}");
                        fullText.AppendLine();
                        fullText.AppendLine("Tracking Data:");
                        fullText.AppendLine();

                        foreach (var t in session.TrackingDataPoints)
                        {
                            fullText.AppendLine($"Timestamp: {t.Timestamp}");
                            if (t.VideoState != null)
                            {
                                fullText.AppendLine($"  VideoState:");
                                fullText.AppendLine($"    Width: {t.VideoState.Width}");
                                fullText.AppendLine($"    Height: {t.VideoState.Height}");
                                fullText.AppendLine($"    X: {t.VideoState.X}");
                                fullText.AppendLine($"    Y: {t.VideoState.Y}");
                                fullText.AppendLine($"    IsFullscreen: {t.VideoState.IsFullscreen}");
                                fullText.AppendLine($"    CurrentTime: {t.VideoState.CurrentTime}");
                                fullText.AppendLine($"    IsPaused: {t.VideoState.IsPaused}");
                            }

                            if (t.Biometrics != null)
                            {
                                fullText.AppendLine($"  Biometrics:");
                                fullText.AppendLine($"    BPM: {t.Biometrics.Bpm}");
                                fullText.AppendLine($"    GSR: {t.Biometrics.Gsr}");
                                fullText.AppendLine($"    AvgBPM: {t.Biometrics.AvgBpm}");
                            }

                            if (t.GazeData != null)
                            {
                                fullText.AppendLine($"  GazeData:");
                                fullText.AppendLine($"    X: {t.GazeData.X}");
                                fullText.AppendLine($"    Y: {t.GazeData.Y}");
                            }

                            fullText.AppendLine();
                        }

                        fileBytes = Encoding.UTF8.GetBytes(fullText.ToString());
                        contentType = "text/plain";
                        break;

                    case "csv":
                        var csvSb = new StringBuilder();
                        csvSb.AppendLine("Timestamp,X,Y,Width,Height,BPM,GSR,AvgBPM");
                        foreach (var t in session.TrackingDataPoints)
                            csvSb.AppendLine($"{t.Timestamp},{t.VideoState?.X},{t.VideoState?.Y},{t.VideoState?.Width},{t.VideoState?.Height},{t.Biometrics?.Bpm},{t.Biometrics?.Gsr},{t.Biometrics?.AvgBpm}");
                        fileBytes = Encoding.UTF8.GetBytes(csvSb.ToString());
                        contentType = "text/csv";
                        break;

                    default:
                        return BadRequest("Invalid format");
                }

                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Download failed: {ex}");
                return StatusCode(500, new { error = ex.Message });
            }
        }


    }
}
