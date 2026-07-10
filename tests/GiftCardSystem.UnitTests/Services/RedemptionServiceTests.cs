using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using GiftCardSystem.Application.DTOs;
using GiftCardSystem.Application.Services;
using GiftCardSystem.Domain.Entities;
using GiftCardSystem.Persistence;

namespace GiftCardSystem.UnitTests.Services
{
    public class RedemptionServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly RedemptionService _service;

        public RedemptionServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new RedemptionService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task RedeemGiftCardAsync_WithValidRequest_ShouldProcessRedemption()
        {
            // Arrange
            var giftCardId = Guid.NewGuid();
            var giftCard = new GiftCard
            {
                Id = giftCardId,
                Code = "ABC123",
                CurrentBalance = 100,
                Status = GiftCardStatus.Active,
                ExpiryDate = DateTime.UtcNow.AddYears(1)
            };

            var request = new CreateRedemptionRequest
            {
                GiftCardCode = "ABC123",
                Amount = 30,
                Description = "Purchase"
            };

            _mockUnitOfWork.Setup(u => u.GiftCards.GetByCodeAsync("ABC123")).ReturnsAsync(giftCard);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.RedeemGiftCardAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(30, result.Amount);
            Assert.Equal("Completed", result.Status);
            Assert.Equal(70, result.RemainingBalance);
        }

        [Fact]
        public async Task RedeemGiftCardAsync_WithInsufficientBalance_ShouldThrowException()
        {
            // Arrange
            var giftCard = new GiftCard
            {
                Code = "ABC123",
                CurrentBalance = 20,
                Status = GiftCardStatus.Active,
                ExpiryDate = DateTime.UtcNow.AddYears(1)
            };

            var request = new CreateRedemptionRequest
            {
                GiftCardCode = "ABC123",
                Amount = 50
            };

            _mockUnitOfWork.Setup(u => u.GiftCards.GetByCodeAsync("ABC123")).ReturnsAsync(giftCard);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RedeemGiftCardAsync(request));
        }

        [Fact]
        public async Task RedeemGiftCardAsync_WithExpiredCard_ShouldThrowException()
        {
            // Arrange
            var giftCard = new GiftCard
            {
                Code = "ABC123",
                CurrentBalance = 100,
                Status = GiftCardStatus.Active,
                ExpiryDate = DateTime.UtcNow.AddDays(-1)
            };

            var request = new CreateRedemptionRequest
            {
                GiftCardCode = "ABC123",
                Amount = 30
            };

            _mockUnitOfWork.Setup(u => u.GiftCards.GetByCodeAsync("ABC123")).ReturnsAsync(giftCard);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RedeemGiftCardAsync(request));
        }

        [Fact]
        public async Task RedeemGiftCardAsync_WithInactiveCard_ShouldThrowException()
        {
            // Arrange
            var giftCard = new GiftCard
            {
                Code = "ABC123",
                CurrentBalance = 100,
                Status = GiftCardStatus.Inactive,
                ExpiryDate = DateTime.UtcNow.AddYears(1)
            };

            var request = new CreateRedemptionRequest
            {
                GiftCardCode = "ABC123",
                Amount = 30
            };

            _mockUnitOfWork.Setup(u => u.GiftCards.GetByCodeAsync("ABC123")).ReturnsAsync(giftCard);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RedeemGiftCardAsync(request));
        }

        [Fact]
        public async Task GetRedemptionAsync_WithValidId_ShouldReturnRedemption()
        {
            // Arrange
            var redemptionId = Guid.NewGuid();
            var giftCardId = Guid.NewGuid();
            var redemption = new Redemption
            {
                Id = redemptionId,
                GiftCardId = giftCardId,
                Amount = 25,
                Status = RedemptionStatus.Completed
            };
            var giftCard = new GiftCard
            {
                Id = giftCardId,
                CurrentBalance = 75
            };

            _mockUnitOfWork.Setup(u => u.Redemptions.GetByIdAsync(redemptionId)).ReturnsAsync(redemption);
            _mockUnitOfWork.Setup(u => u.GiftCards.GetByIdAsync(giftCardId)).ReturnsAsync(giftCard);

            // Act
            var result = await _service.GetRedemptionAsync(redemptionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(25, result.Amount);
            Assert.Equal(75, result.RemainingBalance);
        }
    }
}
