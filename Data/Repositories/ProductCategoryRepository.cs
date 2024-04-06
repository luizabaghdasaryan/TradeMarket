using Data.Data;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories
{
    public class ProductCategoryRepository : AbstractRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(TradeMarketDbContext context) : base(context) { }
    }
}