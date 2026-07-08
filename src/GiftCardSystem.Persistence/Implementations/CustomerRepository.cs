using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GiftCardSystem.Domain.Entities;
using GiftCardSystem.Persistence.Repositories;

namespace GiftCardSystem.Persistence.Implementations
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(GiftCardDbContext context) : base(context)
        {
        }

        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
