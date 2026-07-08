using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiftCardSystem.Domain.Entities;

namespace GiftCardSystem.Persistence.Repositories
{
    public interface IRedemptionRepository : IRepository<Redemption>
    {
        Task<IEnumerable<Redemption>> GetByGiftCardIdAsync(Guid giftCardId);
        Task<IEnumerable<Redemption>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
