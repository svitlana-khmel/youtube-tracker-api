// using EyeTrackingApi.Models;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// [ApiController]
// [Route("api/[controller]")]
// [Authorize]
// public class WeatherController : ControllerBase
// {
//     private readonly ApplicationDbContext _context;

//     public WeatherController(ApplicationDbContext context)
//     {
//         _context = context;
//     }

//     [HttpGet]
//     public async Task<IActionResult> GetWeatherForecasts()
//     {
//         // var forecasts = new[]
//         // {
//         //     new WeatherForecast
//         //     {
//         //         Date = DateOnly.FromDateTime(DateTime.Now),
//         //         TemperatureC = Random.Shared.Next(-20, 55),
//         //         Summary = GetRandomSummary()
//         //     }
//         // };

//         // // Optionally save to database
//         // _context.WeatherForecasts.AddRange(forecasts);
//         // await _context.SaveChangesAsync();

//         //return Ok(forecasts);
//         return Ok();
//     }

//     private static string GetRandomSummary()
//     {
//         var summaries = new[]
//         {
//             "Freezing", "Bracing", "Chilly", "Cool", "Mild",
//             "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//         };

//         return summaries[Random.Shared.Next(summaries.Length)];
//     }

//     public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//     {
//         public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//     }
// }