using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GiftCardSystem.Application.DTOs;
using GiftCardSystem.Application.Services;

namespace GiftCardSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        /// <summary>
        /// Get analytics summary
        /// </summary>
        [HttpGet("summary")]
        [ProducesResponseType(typeof(AnalyticsSummaryResponse), 200)]
        public async Task<ActionResult<AnalyticsSummaryResponse>> GetSummary()
        {
            var response = await _analyticsService.GetSummaryAsync();
            return Ok(response);
        }

        /// <summary>
        /// Generate a report
        /// </summary>
        [HttpGet("reports/{reportType}")]
        [ProducesResponseType(typeof(ReportResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ReportResponse>> GenerateReport(string reportType)
        {
            if (string.IsNullOrEmpty(reportType))
                return BadRequest(new { message = "Report type is required" });

            var response = await _analyticsService.GenerateReportAsync(reportType);
            return Ok(response);
        }
    }
}
