using Data.Interfaces;
using Data.Repositories;
using System;
using System.Threading.Tasks;

namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly TradeMarketDbContext context;
        private ICustomerRepository customerRepository;
        private IPersonRepository personRepository;
        private IProductRepository productRepository;
        private IProductCategoryRepository productCategoryRepository;
        private IReceiptRepository receiptRepository;
        private IReceiptDetailRepository receiptDetailRepository;

        public UnitOfWork(TradeMarketDbContext context)
        {
            this.context = context;
        }

        public ICustomerRepository CustomerRepository => customerRepository ??= new CustomerRepository(context);

        public IPersonRepository PersonRepository => personRepository ??= new PersonRepository(context);

        public IProductRepository ProductRepository => productRepository ??= new ProductRepository(context);

        public IProductCategoryRepository ProductCategoryRepository => productCategoryRepository ??= new ProductCategoryRepository(context);

        public IReceiptRepository ReceiptRepository => receiptRepository ??= new ReceiptRepository(context);

        public IReceiptDetailRepository ReceiptDetailRepository => receiptDetailRepository ??= new ReceiptDetailRepository(context);

        public async Task SaveAsync() => await context.SaveChangesAsync();

        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
