using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CustomerRepository : AbstractRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(TradeMarketDbContext context) : base(context) { }

        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            return await Context.Set<Customer>()
                .Include(c => c.Person)
                .Include(c => c.Receipts)
                    .ThenInclude(r => r.ReceiptDetails)
                        .ThenInclude(rd => rd.Product)
                .ToListAsync();
        }

        public async Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            return await Context.Set<Customer>()
                .Include(c => c.Person)
                .Include(c => c.Receipts)
                    .ThenInclude(r => r.ReceiptDetails)
                        .ThenInclude(rd => rd.Product)
                .SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}