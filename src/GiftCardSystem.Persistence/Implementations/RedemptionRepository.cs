using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GiftCardSystem.Domain.Entities;
using GiftCardSystem.Persistence.Repositories;

namespace GiftCardSystem.Persistence.Implementations
{
    public class RedemptionRepository : Repository<Redemption>, IRedemptionRepository
    {
        public RedemptionRepository(GiftCardDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Redemption>> GetByGiftCardIdAsync(Guid giftCardId)
        {
            return await _dbSet.Where(r => r.GiftCardId == giftCardId).ToListAsync();
        }

        public async Task<IEnumerable<Redemption>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(r => r.RedeemedDate >= startDate && r.RedeemedDate <= endDate).ToListAsync();
        }
    }
}
