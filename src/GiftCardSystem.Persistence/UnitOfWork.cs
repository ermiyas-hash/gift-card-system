using System.Threading.Tasks;
using GiftCardSystem.Persistence.Repositories;
using GiftCardSystem.Persistence.Implementations;

namespace GiftCardSystem.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GiftCardDbContext _context;
        private IGiftCardRepository _giftCardRepository;
        private IRedemptionRepository _redemptionRepository;
        private ITransactionRepository _transactionRepository;
        private ICustomerRepository _customerRepository;

        public UnitOfWork(GiftCardDbContext context)
        {
            _context = context;
        }

        public IGiftCardRepository GiftCards
        {
            get { return _giftCardRepository ??= new GiftCardRepository(_context); }
        }

        public IRedemptionRepository Redemptions
        {
            get { return _redemptionRepository ??= new RedemptionRepository(_context); }
        }

        public ITransactionRepository Transactions
        {
            get { return _transactionRepository ??= new TransactionRepository(_context); }
        }

        public ICustomerRepository Customers
        {
            get { return _customerRepository ??= new CustomerRepository(_context); }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
