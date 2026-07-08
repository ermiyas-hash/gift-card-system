using System.Threading.Tasks;
using GiftCardSystem.Application.DTOs;

namespace GiftCardSystem.Application.Services
{
    public interface IAnalyticsService
    {
        Task<AnalyticsSummaryResponse> GetSummaryAsync();
        Task<ReportResponse> GenerateReportAsync(string reportType);
    }
}
