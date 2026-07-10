using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using GiftCardSystem.API.Controllers;
using GiftCardSystem.Application.DTOs;
using GiftCardSystem.Application.Services;

namespace GiftCardSystem.UnitTests.Controllers
{
    public class AnalyticsControllerTests
    {
        private readonly Mock<IAnalyticsService> _mockAnalyticsService;
        private readonly AnalyticsController _controller;

        public AnalyticsControllerTests()
        {
            _mockAnalyticsService = new Mock<IAnalyticsService>();
            _controller = new AnalyticsController(_mockAnalyticsService.Object);
        }

        [Fact]
        public async Task GetSummary_ShouldReturnOkResult()
        {
            // Arrange
            var response = new AnalyticsSummaryResponse
            {
                TotalGiftCards = 10,
                ActiveGiftCards = 8,
                TotalIssuedAmount = 1000,
                TotalRedeemedAmount = 500
            };

            _mockAnalyticsService.Setup(s => s.GetSummaryAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetSummary();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResponse = Assert.IsType<AnalyticsSummaryResponse>(okResult.Value);
            Assert.Equal(10, returnedResponse.TotalGiftCards);
        }

        [Fact]
        public async Task GenerateReport_WithValidReportType_ShouldReturnOkResult()
        {
            // Arrange
            var reportType = "Summary";
            var response = new ReportResponse
            {
                Id = Guid.NewGuid(),
                ReportType = reportType,
                GeneratedDate = DateTime.UtcNow
            };

            _mockAnalyticsService.Setup(s => s.GenerateReportAsync(reportType)).ReturnsAsync(response);

            // Act
            var result = await _controller.GenerateReport(reportType);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GenerateReport_WithEmptyReportType_ShouldReturnBadRequest()
        {
            // Act
            var result = await _controller.GenerateReport("");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
