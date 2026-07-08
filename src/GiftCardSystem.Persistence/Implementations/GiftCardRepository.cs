using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GiftCardSystem.Domain.Entities;
using GiftCardSystem.Persistence.Repositories;

namespace GiftCardSystem.Persistence.Implementations
{
    public class GiftCardRepository : Repository<GiftCard>, IGiftCardRepository
    {
        public GiftCardRepository(GiftCardDbContext context) : base(context)
        {
        }

        public async Task<GiftCard> GetByCodeAsync(string code)
        {
            return await _dbSet.FirstOrDefaultAsync(g => g.Code == code);
        }

        public async Task<IEnumerable<GiftCard>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _dbSet.Where(g => g.CustomerId == customerId).ToListAsync();
        }

        public async Task<IEnumerable<GiftCard>> GetExpiredCardsAsync()
        {
            var now = DateTime.UtcNow;
            return await _dbSet.Where(g => g.ExpiryDate.HasValue && g.ExpiryDate.Value < now).ToListAsync();
        }
    }
}
