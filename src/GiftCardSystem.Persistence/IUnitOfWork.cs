using System.Threading.Tasks;
using GiftCardSystem.Persistence.Repositories;

namespace GiftCardSystem.Persistence
{
    public interface IUnitOfWork
    {
        IGiftCardRepository GiftCards { get; }
        IRedemptionRepository Redemptions { get; }
        ITransactionRepository Transactions { get; }
        ICustomerRepository Customers { get; }

        Task<int> SaveChangesAsync();
        void Dispose();
    }
}
