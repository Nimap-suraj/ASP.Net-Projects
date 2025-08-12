using GenericRepositoryPattern.Data;
using GenericRepositoryPattern.Entity;
using GenericRepositoryPattern.Repositories.Interface;

namespace GenericRepositoryPattern.Repositories.Implementation
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<string> GetMaximumProductPrice(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
