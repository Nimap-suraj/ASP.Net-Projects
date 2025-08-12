using GenericRepositoryPattern.Data;
using GenericRepositoryPattern.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GenericRepositoryPattern.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly  DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity) =>
             await _dbSet.AddAsync(entity);


        public void Delete(T entity)
           =>  _dbSet.Remove(entity);

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsyc(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(T entity)
         => _dbSet.Update(entity);
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
    }
}
