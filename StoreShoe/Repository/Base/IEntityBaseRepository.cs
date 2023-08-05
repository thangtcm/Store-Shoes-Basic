using System.Linq.Expressions;

namespace StoreShoe.Repository.Base
{
    public interface IEntityBaseRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsyn();

        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(int id, T entity);

        Task DeleteAsync(int id);
    }
}
