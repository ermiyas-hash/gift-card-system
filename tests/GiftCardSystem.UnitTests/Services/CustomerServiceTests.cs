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
    public class CustomerServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new CustomerService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task CreateCustomerAsync_WithValidRequest_ShouldCreateCustomer()
        {
            // Arrange
            var request = new CreateCustomerRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "123-456-7890"
            };

            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.CreateCustomerAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Equal("john@example.com", result.Email);
        }

        [Fact]
        public async Task GetCustomerAsync_WithValidId_ShouldReturnCustomerWithGiftCards()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                Id = customerId,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                CreatedAt = DateTime.UtcNow
            };

            var giftCards = new List<GiftCard>
            {
                new GiftCard
                {
                    Id = Guid.NewGuid(),
                    Code = "GC001",
                    InitialBalance = 100,
                    CurrentBalance = 75,
                    Status = GiftCardStatus.Active
                }
            };

            _mockUnitOfWork.Setup(u => u.Customers.GetByIdAsync(customerId)).ReturnsAsync(customer);
            _mockUnitOfWork.Setup(u => u.GiftCards.GetByCustomerIdAsync(customerId)).ReturnsAsync(giftCards);

            // Act
            var result = await _service.GetCustomerAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane", result.FirstName);
            Assert.Single(result.GiftCards);
        }

        [Fact]
        public async Task GetCustomerAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _mockUnitOfWork.Setup(u => u.Customers.GetByIdAsync(customerId)).ReturnsAsync((Customer)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetCustomerAsync(customerId));
        }

        [Fact]
        public async Task UpdateCustomerAsync_WithValidRequest_ShouldUpdateCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                Id = customerId,
                FirstName = "Old",
                LastName = "Name",
                Email = "old@example.com"
            };

            var request = new CreateCustomerRequest
            {
                FirstName = "New",
                LastName = "Name",
                Email = "new@example.com",
                PhoneNumber = "987-654-3210"
            };

            _mockUnitOfWork.Setup(u => u.Customers.GetByIdAsync(customerId)).ReturnsAsync(customer);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.UpdateCustomerAsync(customerId, request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New", result.FirstName);
            Assert.Equal("new@example.com", result.Email);
        }

        [Fact]
        public async Task DeleteCustomerAsync_WithValidId_ShouldDeleteCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer { Id = customerId };

            _mockUnitOfWork.Setup(u => u.Customers.GetByIdAsync(customerId)).ReturnsAsync(customer);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.DeleteCustomerAsync(customerId);

            // Assert
            Assert.True(result);
            _mockUnitOfWork.Verify(u => u.Customers.Delete(customer), Times.Once);
        }
    }
}
