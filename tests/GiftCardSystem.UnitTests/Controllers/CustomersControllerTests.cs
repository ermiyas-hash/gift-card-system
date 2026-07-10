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
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerService> _mockCustomerService;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _controller = new CustomersController(_mockCustomerService.Object);
        }

        [Fact]
        public async Task CreateCustomer_WithValidRequest_ShouldReturnCreatedResult()
        {
            // Arrange
            var request = new CreateCustomerRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com"
            };
            var response = new CustomerResponse
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com"
            };

            _mockCustomerService.Setup(s => s.CreateCustomerAsync(request)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateCustomer(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(CustomersController.GetCustomer), createdResult.ActionName);
        }

        [Fact]
        public async Task GetCustomer_WithValidId_ShouldReturnOkResult()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var response = new CustomerDetailResponse
            {
                Id = customerId,
                FirstName = "Jane",
                LastName = "Smith",
                GiftCards = new List<GiftCardResponse>()
            };

            _mockCustomerService.Setup(s => s.GetCustomerAsync(customerId)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetCustomer(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(customerId, ((CustomerDetailResponse)okResult.Value).Id);
        }

        [Fact]
        public async Task GetCustomer_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _mockCustomerService.Setup(s => s.GetCustomerAsync(customerId))
                .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.GetCustomer(customerId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetAllCustomers_ShouldReturnOkResult()
        {
            // Arrange
            var response = new List<CustomerResponse>
            {
                new CustomerResponse { Id = Guid.NewGuid(), FirstName = "John" },
                new CustomerResponse { Id = Guid.NewGuid(), FirstName = "Jane" }
            };

            _mockCustomerService.Setup(s => s.GetAllCustomersAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(2, ((List<CustomerResponse>)okResult.Value).Count);
        }

        [Fact]
        public async Task UpdateCustomer_WithValidRequest_ShouldReturnOkResult()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var request = new CreateCustomerRequest
            {
                FirstName = "Updated",
                LastName = "Name"
            };
            var response = new CustomerResponse
            {
                Id = customerId,
                FirstName = "Updated",
                LastName = "Name"
            };

            _mockCustomerService.Setup(s => s.UpdateCustomerAsync(customerId, request)).ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateCustomer(customerId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("Updated", ((CustomerResponse)okResult.Value).FirstName);
        }

        [Fact]
        public async Task DeleteCustomer_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _mockCustomerService.Setup(s => s.DeleteCustomerAsync(customerId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCustomer(customerId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
