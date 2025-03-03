
using System.Linq.Expressions;
using AEL.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace AEL.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity<int>
    {
        Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        Task<T> GetByIdAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
        Task<T> GetByIdAsync(int id, bool includeRelatedEntities);
    }

}
