using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using EyeTrackingApi.Services;
using EyeTrackingApi.Models;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateReport([FromBody] EyeTrackingData data)
    {
        try
        {
            var pdfBytes = await _reportService.GenerateReportAsync(data);
            return File(pdfBytes, "application/pdf", "eye-tracking-report.pdf");
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}