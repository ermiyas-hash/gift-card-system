using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiftCardSystem.Domain.Entities;

namespace GiftCardSystem.Persistence.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByEmailAsync(string email);
    }
}
