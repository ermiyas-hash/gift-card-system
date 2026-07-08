using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiftCardSystem.Domain.Entities;

namespace GiftCardSystem.Persistence.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByGiftCardIdAsync(Guid giftCardId);
        Task<IEnumerable<Transaction>> GetByTypeAsync(TransactionType type);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
