using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ReceiptRepository : AbstractRepository<Receipt>, IReceiptRepository
    {
        public ReceiptRepository(TradeMarketDbContext context) : base(context) { }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await Context.Set<Receipt>()
                .Include(r => r.Customer)
                    .ThenInclude(c => c.Person)
                .Include(r => r.ReceiptDetails)
                    .ThenInclude(rd => rd.Product)
                        .ThenInclude(p => p.Category)
                .ToListAsync();
        }

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return await Context.Set<Receipt>()
                .Include(r => r.Customer)
                    .ThenInclude(c => c.Person)
                .Include(r => r.ReceiptDetails)
                    .ThenInclude(rd => rd.Product)
                        .ThenInclude(p => p.Category)
                .SingleOrDefaultAsync(r => r.Id == id);
        }
    }
}