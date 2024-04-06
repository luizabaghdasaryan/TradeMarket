using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductRepository : AbstractRepository<Product>, IProductRepository
    {

        public ProductRepository(TradeMarketDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await Context.Set<Product>()
                .Include(p => p.Category)
                .Include(p => p.ReceiptDetails)
                .ToArrayAsync();
        }

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return await Context.Set<Product>()
                .Include(p => p.Category)
                .Include(p => p.ReceiptDetails)
                .SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}