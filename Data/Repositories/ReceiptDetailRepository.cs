using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ReceiptDetailRepository : AbstractRepository<ReceiptDetail>, IReceiptDetailRepository
    {
        public ReceiptDetailRepository(TradeMarketDbContext context) : base(context) { }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await Context.Set<ReceiptDetail>()
                .Include(rd => rd.Receipt)
                    .ThenInclude(r => r.Customer)
                .Include(rd => rd.Product)
                    .ThenInclude(p => p.Category)
                .ToListAsync();
        }
    }
}