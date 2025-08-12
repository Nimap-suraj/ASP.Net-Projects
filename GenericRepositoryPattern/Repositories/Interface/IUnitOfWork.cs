using GenericRepositoryPattern.Entity;

namespace GenericRepositoryPattern.Repositories.Interface
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IProductRepository products { get; }
        Task<int> SaveChangesAsync();
    }
}
