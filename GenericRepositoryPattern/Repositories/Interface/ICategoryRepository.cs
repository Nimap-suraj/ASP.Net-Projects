using GenericRepositoryPattern.Entity;

namespace GenericRepositoryPattern.Repositories.Interface
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetCategoryWithProductsAsync(int id);

    }
}
