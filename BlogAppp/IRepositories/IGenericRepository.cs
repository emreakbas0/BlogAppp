using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlogAppp.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> FindAsync(int id);
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> Where(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
