using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiftCardSystem.Domain.Entities;

namespace GiftCardSystem.Persistence.Repositories
{
    public interface IGiftCardRepository : IRepository<GiftCard>
    {
        Task<GiftCard> GetByCodeAsync(string code);
        Task<IEnumerable<GiftCard>> GetByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<GiftCard>> GetExpiredCardsAsync();
    }
}
