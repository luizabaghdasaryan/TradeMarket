using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public abstract class AbstractRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly TradeMarketDbContext context;

        protected TradeMarketDbContext Context => context;

        protected AbstractRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);

            return entity;
        }

        public async Task AddAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }
        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id) ?? throw new ArgumentException("Invalid id");
            Delete(entity);
        }

        public void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }
    }
}