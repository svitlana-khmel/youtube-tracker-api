using EyeTrackingApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using EyeTrackingApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace EyeTrackingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TrackingController : ControllerBase
    {
        // POST: /api/tracking/sav
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

                if (session == null)
                    return NotFound();

                byte[] fileBytes;
                string contentType;
                string fileName = $"session-{id}.{format}";

                switch (format.ToLower())
                {
                    case "json":
                        var dto = new SessionDto
                        {
                            Id = session.Id,
                            TrackingDataPoints = session.TrackingDataPoints.Select(t => new TrackingPointDto
                            {
                                Timestamp = t.Timestamp,
                                X = t.VideoState?.X,
                                Y = t.VideoState?.Y,
                                Width = t.VideoState?.Width,
                                Height = t.VideoState?.Height,
                                Bpm = t.Biometrics?.Bpm,
                                Gsr = t.Biometrics?.Gsr,
                                AvgBpm = t.Biometrics?.AvgBpm
                            }).ToList()
                        };

                        fileBytes = Encoding.UTF8.GetBytes(
                            JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true })
                        );
                        contentType = "application/json"; // ✅ Added this line
                        break;

                    case "txt":
                        var sb = new StringBuilder();
                        foreach (var t in session.TrackingDataPoints)
                            sb.AppendLine($"{t.Timestamp}: x={t.VideoState?.X ?? 0}, y={t.VideoState?.Y ?? 0}, bpm={t.Biometrics?.Bpm ?? 0}");
                        fileBytes = Encoding.UTF8.GetBytes(sb.ToString());
                        contentType = "text/plain";
                        break;

                    case "csv":
                        var csvSb = new StringBuilder();
                        csvSb.AppendLine("Timestamp,X,Y,Width,Height,BPM,GSR,AvgBPM");
                        foreach (var t in session.TrackingDataPoints)
                            csvSb.AppendLine($"{t.Timestamp},{t.VideoState?.X ?? 0},{t.VideoState?.Y ?? 0},{t.VideoState?.Width ?? 0},{t.VideoState?.Height ?? 0},{t.Biometrics?.Bpm ?? 0},{t.Biometrics?.Gsr ?? 0},{t.Biometrics?.AvgBpm ?? 0}");
                        fileBytes = Encoding.UTF8.GetBytes(csvSb.ToString());
                        contentType = "text/csv";
                        break;

                    case "zip":
                        byte[] zipBytes;
                        using (var ms = new MemoryStream())
                        {
                            using (var archive = new System.IO.Compression.ZipArchive(ms, System.IO.Compression.ZipArchiveMode.Create, true))
                            {
                                var entry = archive.CreateEntry($"session-{id}.json");
                                using var entryStream = entry.Open();
                                using var writer = new StreamWriter(entryStream);
                                writer.Write(JsonSerializer.Serialize(session, new JsonSerializerOptions { WriteIndented = true }));
                            }

                            zipBytes = ms.ToArray(); // capture bytes inside using
                        }

                        fileBytes = zipBytes;
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
