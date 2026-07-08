using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiftCardSystem.Application.DTOs;
using GiftCardSystem.Domain.Entities;
using GiftCardSystem.Persistence;

namespace GiftCardSystem.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(customer);
        }

        public async Task<CustomerDetailResponse> GetCustomerAsync(Guid id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found");

            var giftCards = (await _unitOfWork.GiftCards.GetByCustomerIdAsync(id))
                .Select(gc => new GiftCardResponse
                {
                    Id = gc.Id,
                    Code = gc.Code,
                    InitialBalance = gc.InitialBalance,
                    CurrentBalance = gc.CurrentBalance,
                    IssuedDate = gc.IssuedDate,
                    ExpiryDate = gc.ExpiryDate,
                    Status = gc.Status.ToString(),
                    CustomerId = gc.CustomerId,
                    CreatedAt = gc.CreatedAt
                })
                .ToList();

            return new CustomerDetailResponse
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                GiftCards = giftCards,
                CreatedAt = customer.CreatedAt
            };
        }

        public async Task<IEnumerable<CustomerResponse>> GetAllCustomersAsync()
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();
            return customers.Select(MapToResponse).ToList();
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(Guid id, CreateCustomerRequest request)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found");

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
            customer.PhoneNumber = request.PhoneNumber;
            customer.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(customer);
        }

        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
                return false;

            _unitOfWork.Customers.Delete(customer);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private CustomerResponse MapToResponse(Customer customer)
        {
            return new CustomerResponse
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                CreatedAt = customer.CreatedAt
            };
        }
    }
}
