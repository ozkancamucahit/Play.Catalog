

using Common.Lib.Entities;
using System.Linq.Expressions;

namespace Common.Lib.Repositories
{
    public interface IRepository<T> where T: IEntity
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> Filter);
        Task<T> GetAsync(Guid id);
        Task<T> GetAsync(Expression<Func<T, bool>> Filter);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(T entity);
    }
}