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
    public class GiftCardsControllerTests
    {
        private readonly Mock<IGiftCardService> _mockGiftCardService;
        private readonly GiftCardsController _controller;

        public GiftCardsControllerTests()
        {
            _mockGiftCardService = new Mock<IGiftCardService>();
            _controller = new GiftCardsController(_mockGiftCardService.Object);
        }

        [Fact]
        public async Task CreateGiftCard_WithValidRequest_ShouldReturnCreatedResult()
        {
            // Arrange
            var request = new CreateGiftCardRequest { InitialBalance = 100 };
            var response = new GiftCardResponse
            {
                Id = Guid.NewGuid(),
                Code = "ABC123",
                InitialBalance = 100,
                CurrentBalance = 100,
                Status = "Active"
            };

            _mockGiftCardService.Setup(s => s.CreateGiftCardAsync(request)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateGiftCard(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(GiftCardsController.GetGiftCard), createdResult.ActionName);
            Assert.Equal(response.Id, ((GiftCardResponse)createdResult.Value).Id);
        }

        [Fact]
        public async Task GetGiftCard_WithValidId_ShouldReturnOkResult()
        {
            // Arrange
            var giftCardId = Guid.NewGuid();
            var response = new GiftCardResponse
            {
                Id = giftCardId,
                Code = "ABC123",
                Status = "Active"
            };

            _mockGiftCardService.Setup(s => s.GetGiftCardAsync(giftCardId)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetGiftCard(giftCardId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(response.Id, ((GiftCardResponse)okResult.Value).Id);
        }

        [Fact]
        public async Task GetGiftCard_WithInvalidId_ShouldReturnNotFoundResult()
        {
            // Arrange
            var giftCardId = Guid.NewGuid();
            _mockGiftCardService.Setup(s => s.GetGiftCardAsync(giftCardId))
                .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.GetGiftCard(giftCardId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetAllGiftCards_ShouldReturnOkResult()
        {
            // Arrange
            var response = new List<GiftCardResponse>
            {
                new GiftCardResponse { Id = Guid.NewGuid(), Code = "GC001" },
                new GiftCardResponse { Id = Guid.NewGuid(), Code = "GC002" }
            };

            _mockGiftCardService.Setup(s => s.GetAllGiftCardsAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllGiftCards();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedList = Assert.IsAssignableFrom<IEnumerable<GiftCardResponse>>(okResult.Value);
            Assert.Equal(2, returnedList.Count());
        }

        [Fact]
        public async Task DeleteGiftCard_WithValidId_ShouldReturnNoContentResult()
        {
            // Arrange
            var giftCardId = Guid.NewGuid();
            _mockGiftCardService.Setup(s => s.DeleteGiftCardAsync(giftCardId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteGiftCard(giftCardId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetBalance_WithValidId_ShouldReturnBalance()
        {
            // Arrange
            var giftCardId = Guid.NewGuid();
            _mockGiftCardService.Setup(s => s.GetBalanceAsync(giftCardId)).ReturnsAsync(75.50m);

            // Act
            var result = await _controller.GetBalance(giftCardId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }
    }
}
