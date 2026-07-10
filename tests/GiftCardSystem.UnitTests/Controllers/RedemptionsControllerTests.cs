using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using GiftCardSystem.API.Controllers;
using GiftCardSystem.Application.DTOs;
using GiftCardSystem.Application.Services;

namespace GiftCardSystem.UnitTests.Controllers
{
    public class RedemptionsControllerTests
    {
        private readonly Mock<IRedemptionService> _mockRedemptionService;
        private readonly RedemptionsController _controller;

        public RedemptionsControllerTests()
        {
            _mockRedemptionService = new Mock<IRedemptionService>();
            _controller = new RedemptionsController(_mockRedemptionService.Object);
        }

        [Fact]
        public async Task RedeemGiftCard_WithValidRequest_ShouldReturnCreatedResult()
        {
            // Arrange
            var request = new CreateRedemptionRequest
            {
                GiftCardCode = "ABC123",
                Amount = 50
            };
            var response = new RedemptionResponse
            {
                Id = Guid.NewGuid(),
                Amount = 50,
                Status = "Completed",
                RemainingBalance = 50
            };

            _mockRedemptionService.Setup(s => s.RedeemGiftCardAsync(request)).ReturnsAsync(response);

            // Act
            var result = await _controller.RedeemGiftCard(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(RedemptionsController.GetRedemption), createdResult.ActionName);
        }

        [Fact]
        public async Task RedeemGiftCard_WithInvalidGiftCard_ShouldReturnNotFound()
        {
            // Arrange
            var request = new CreateRedemptionRequest
            {
                GiftCardCode = "INVALID",
                Amount = 50
            };

            _mockRedemptionService.Setup(s => s.RedeemGiftCardAsync(request))
                .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.RedeemGiftCard(request);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task RedeemGiftCard_WithInsufficientBalance_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new CreateRedemptionRequest
            {
                GiftCardCode = "ABC123",
                Amount = 500
            };

            _mockRedemptionService.Setup(s => s.RedeemGiftCardAsync(request))
                .ThrowsAsync(new InvalidOperationException("Insufficient balance"));

            // Act
            var result = await _controller.RedeemGiftCard(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetRedemption_WithValidId_ShouldReturnOkResult()
        {
            // Arrange
            var redemptionId = Guid.NewGuid();
            var response = new RedemptionResponse
            {
                Id = redemptionId,
                Amount = 50,
                Status = "Completed"
            };

            _mockRedemptionService.Setup(s => s.GetRedemptionAsync(redemptionId)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetRedemption(redemptionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(redemptionId, ((RedemptionResponse)okResult.Value).Id);
        }

        [Fact]
        public async Task GetAllRedemptions_ShouldReturnOkResult()
        {
            // Arrange
            var response = new List<RedemptionResponse>
            {
                new RedemptionResponse { Id = Guid.NewGuid(), Amount = 50 },
                new RedemptionResponse { Id = Guid.NewGuid(), Amount = 30 }
            };

            _mockRedemptionService.Setup(s => s.GetAllRedemptionsAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllRedemptions();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(2, ((List<RedemptionResponse>)okResult.Value).Count);
        }
    }
}
