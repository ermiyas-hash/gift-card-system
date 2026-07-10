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
    public class GiftCardServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly GiftCardService _service;

        public GiftCardServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new GiftCardService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task CreateGiftCardAsync_WithValidRequest_ShouldCreateGiftCard()
        {
            // Arrange
            var request = new CreateGiftCardRequest
            {
                InitialBalance = 100,
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                CustomerId = Guid.NewGuid()
            };

            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.CreateGiftCardAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.InitialBalance);
            Assert.Equal(100, result.CurrentBalance);
            Assert.NotEmpty(result.Code);
            Assert.Equal("Active", result.Status);
        }

        [Fact]
        public async Task GetGiftCardAsync_WithValidId_ShouldReturnGiftCard()
        {
            // Arrange
            var giftCardId = Guid.NewGuid();
            var giftCard = new GiftCard
            {
                Id = giftCardId,
                Code = "ABC123",
                InitialBalance = 50,
                CurrentBalance = 50,
                Status = GiftCardStatus.Active,
                IssuedDate = DateTime.UtcNow
            };

            _mockUnitOfWork.Setup(u => u.GiftCards.GetByIdAsync(giftCardId)).ReturnsAsync(giftCard);

            // Act
            var result = await _service.GetGiftCardAsync(giftCardId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(giftCardId, result.Id);
            Assert.Equal("ABC123", result.Code);
        }

        [Fact]
        public async Task GetGiftCardAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var giftCardId = Guid.NewGuid();
            _mockUnitOfWork.Setup(u => u.GiftCards.GetByIdAsync(giftCardId)).ReturnsAsync((GiftCard)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetGiftCardAsync(giftCardId));
        }

        [Fact]
        public async Task GetBalanceAsync_WithValidId_ShouldReturnBalance()
        {
            // Arrange
            var giftCardId = Guid.NewGuid();
            var giftCard = new GiftCard
            {
                Id = giftCardId,
                CurrentBalance = 75.50m
            };

            _mockUnitOfWork.Setup(u => u.GiftCards.GetByIdAsync(giftCardId)).ReturnsAsync(giftCard);

            // Act
            var result = await _service.GetBalanceAsync(giftCardId);

            // Assert
            Assert.Equal(75.50m, result);
        }

        [Fact]
        public async Task DeleteGiftCardAsync_WithValidId_ShouldDeleteGiftCard()
        {
            // Arrange
            var giftCardId = Guid.NewGuid();
            var giftCard = new GiftCard { Id = giftCardId };

            _mockUnitOfWork.Setup(u => u.GiftCards.GetByIdAsync(giftCardId)).ReturnsAsync(giftCard);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.DeleteGiftCardAsync(giftCardId);

            // Assert
            Assert.True(result);
            _mockUnitOfWork.Verify(u => u.GiftCards.Delete(giftCard), Times.Once);
        }
    }
}
