using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiftCardSystem.Application.DTOs;

namespace GiftCardSystem.Application.Services
{
    public interface ICustomerService
    {
        Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request);
        Task<CustomerDetailResponse> GetCustomerAsync(Guid id);
        Task<IEnumerable<CustomerResponse>> GetAllCustomersAsync();
        Task<CustomerResponse> UpdateCustomerAsync(Guid id, CreateCustomerRequest request);
        Task<bool> DeleteCustomerAsync(Guid id);
    }
}
