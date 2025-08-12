using GenericRepositoryPattern.Entity;

namespace GenericRepositoryPattern.Repositories.Interface
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<string> GetMaximumProductPrice(Product product);
    }
}
