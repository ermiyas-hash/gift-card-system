using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using GiftCardSystem.Application.DTOs;
using GiftCardSystem.Application.Services;
using GiftCardSystem.Domain.Entities;
using GiftCardSystem.Persistence;

namespace GiftCardSystem.UnitTests.Services
{
    public class AnalyticsServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly AnalyticsService _service;

        public AnalyticsServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new AnalyticsService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetSummaryAsync_ShouldReturnCorrectStatistics()
        {
            // Arrange
            var giftCards = new List<GiftCard>
            {
                new GiftCard { Id = Guid.NewGuid(), Status = GiftCardStatus.Active, CurrentBalance = 100 },
                new GiftCard { Id = Guid.NewGuid(), Status = GiftCardStatus.Active, CurrentBalance = 50 },
                new GiftCard { Id = Guid.NewGuid(), Status = GiftCardStatus.Expired, CurrentBalance = 0 }
            };

            var transactions = new List<Transaction>
            {
                new Transaction { Amount = 100, Type = TransactionType.Issuance },
                new Transaction { Amount = 50, Type = TransactionType.Issuance },
                new Transaction { Amount = 30, Type = TransactionType.Redemption }
            };

            var customers = new List<Customer>
            {
                new Customer { Id = Guid.NewGuid() },
                new Customer { Id = Guid.NewGuid() }
            };

            _mockUnitOfWork.Setup(u => u.GiftCards.GetAllAsync()).ReturnsAsync(giftCards);
            _mockUnitOfWork.Setup(u => u.Transactions.GetAllAsync()).ReturnsAsync(transactions);
            _mockUnitOfWork.Setup(u => u.Customers.GetAllAsync()).ReturnsAsync(customers);

            // Act
            var result = await _service.GetSummaryAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalGiftCards);
            Assert.Equal(2, result.ActiveGiftCards);
            Assert.Equal(1, result.ExpiredGiftCards);
            Assert.Equal(150, result.TotalIssuedAmount);
            Assert.Equal(30, result.TotalRedeemedAmount);
            Assert.Equal(150, result.TotalRemainingBalance);
            Assert.Equal(2, result.TotalCustomers);
        }

        [Fact]
        public async Task GenerateReportAsync_ShouldReturnReport()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.GiftCards.GetAllAsync()).ReturnsAsync(new List<GiftCard>());
            _mockUnitOfWork.Setup(u => u.Transactions.GetAllAsync()).ReturnsAsync(new List<Transaction>());
            _mockUnitOfWork.Setup(u => u.Customers.GetAllAsync()).ReturnsAsync(new List<Customer>());

            // Act
            var result = await _service.GenerateReportAsync("Summary");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Summary", result.ReportType);
            Assert.NotNull(result.ReportData);
        }
    }
}
