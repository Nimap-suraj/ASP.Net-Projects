using GenericRepositoryPattern.Data;
using GenericRepositoryPattern.Entity;
using GenericRepositoryPattern.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryPattern.Repositories.Implementation
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context) : base(context) {
            _context = context;
        }

        public async Task<Category> GetCategoryWithProductsAsync(int id)
        {
            return await _context.Categories
                                 .Include(c => c.Products)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
