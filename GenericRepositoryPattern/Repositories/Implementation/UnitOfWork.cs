using GenericRepositoryPattern.Data;
using GenericRepositoryPattern.Entity;
using GenericRepositoryPattern.Repositories.Interface;

namespace GenericRepositoryPattern.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICategoryRepository Categories { get; }
        public IProductRepository products { get; }

        public UnitOfWork(AppDbContext context, ICategoryRepository categoryRepository, IProductRepository product)
        {
            _context = context;
            Categories = categoryRepository;
            products = product;
        }
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
