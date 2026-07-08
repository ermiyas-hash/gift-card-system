using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GiftCardSystem.Domain.Entities;
using GiftCardSystem.Persistence.Repositories;

namespace GiftCardSystem.Persistence.Implementations
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(GiftCardDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Transaction>> GetByGiftCardIdAsync(Guid giftCardId)
        {
            return await _dbSet.Where(t => t.GiftCardId == giftCardId).ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByTypeAsync(TransactionType type)
        {
            return await _dbSet.Where(t => t.Type == type).ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).ToListAsync();
        }
    }
}
