
using AEL.Domain.Entities;

namespace AEL.Application.Interfaces.Services
{
    public interface IGenericService<T> where T : BaseEntity<int>
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }

}
