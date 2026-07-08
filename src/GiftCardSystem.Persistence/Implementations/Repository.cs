using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GiftCardSystem.Domain.Entities;
using GiftCardSystem.Persistence.Repositories;

namespace GiftCardSystem.Persistence.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly GiftCardDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(GiftCardDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
