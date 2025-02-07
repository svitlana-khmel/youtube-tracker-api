using EyeTrackingApi.Models;

namespace EyeTrackingApi.Services;
public interface IReportService
{
    Task<byte[]> GenerateReportAsync(EyeTrackingData data);
    //AnalysisResult AnalyzeData(EyeTrackingData data);
}