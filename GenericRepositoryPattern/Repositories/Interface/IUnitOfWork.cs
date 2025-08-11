using GenericRepositoryPattern.Entity;

namespace GenericRepositoryPattern.Repositories.Interface
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        Task<int> SaveChangesAsync();
    }
}
