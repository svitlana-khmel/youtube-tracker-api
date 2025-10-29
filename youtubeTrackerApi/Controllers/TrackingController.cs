using EyeTrackingApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SessionData sessionData)
        {
            if (sessionData == null || sessionData.TrackingDataPoints.Count == 0)
                return BadRequest(new { ok = false, error = "No session data provided" });

            // Attach user from JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
                sessionData.UserId = userId;

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
                        var jsonExport = new
                        {
                            pageUrl = session.PageUrl,
                            startTime = session.StartTime,
                            trackingData = session.TrackingDataPoints.Select(t => new
                            {
                                timestamp = t.Timestamp,
                                videoState = new
                                {
                                    t.VideoState.Width,
                                    t.VideoState.Height,
                                    t.VideoState.X,
                                    t.VideoState.Y,
                                    t.VideoState.IsFullscreen,
                                    t.VideoState.CurrentTime,
                                    t.VideoState.IsPaused
                                },
                                biometrics = new
                                {
                                    t.Biometrics.Bpm,
                                    t.Biometrics.Gsr,
                                    t.Biometrics.AvgBpm
                                },
                                gazeData = t.GazeData == null ? null : new
                                {
                                    t.GazeData.X,
                                    t.GazeData.Y
                                }
                            })
                        };

                        fileBytes = Encoding.UTF8.GetBytes(
                            JsonSerializer.Serialize(jsonExport, new JsonSerializerOptions
                            {
                                WriteIndented = true
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
                        csvSb.AppendLine("PageUrl,StartTime,Timestamp,Width,Height,X,Y,IsFullscreen,CurrentTime,IsPaused,BPM,GSR,AvgBPM,GazeX,GazeY");

                        foreach (var t in session.TrackingDataPoints)
                        {
                            csvSb.AppendLine($"{session.PageUrl},{session.StartTime},{t.Timestamp}," +
                                $"{t.VideoState?.Width ?? 0},{t.VideoState?.Height ?? 0},{t.VideoState?.X ?? 0},{t.VideoState?.Y ?? 0}," +
                                $"{t.VideoState?.IsFullscreen ?? false},{t.VideoState?.CurrentTime ?? 0},{t.VideoState?.IsPaused ?? false}," +
                                $"{t.Biometrics?.Bpm ?? -1},{t.Biometrics?.Gsr ?? -1},{t.Biometrics?.AvgBpm ?? -1}," +
                                $"{t.GazeData?.X ?? 0},{t.GazeData?.Y ?? 0}");
                        }

                        fileBytes = Encoding.UTF8.GetBytes(csvSb.ToString());
                        contentType = "text/csv";
                        break;
                    case "zip":
                        using (var ms = new MemoryStream())
                        {
                            using (var archive = new System.IO.Compression.ZipArchive(ms, System.IO.Compression.ZipArchiveMode.Create, true))
                            {
                                var entry = archive.CreateEntry($"session-{id}.json");
                                using var entryStream = entry.Open();
                                using var writer = new StreamWriter(entryStream);

                                var zipJson = new
                                {
                                    pageUrl = session.PageUrl,
                                    startTime = session.StartTime,
                                    trackingData = session.TrackingDataPoints.Select(t => new
                                    {
                                        timestamp = t.Timestamp,
                                        videoState = t.VideoState,
                                        biometrics = t.Biometrics,
                                        gazeData = t.GazeData
                                    })
                                };

                                writer.Write(JsonSerializer.Serialize(zipJson, new JsonSerializerOptions
                                {
                                    WriteIndented = true
                                }));
                            }

                            fileBytes = ms.ToArray();
                        }

                        contentType = "application/zip";
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
