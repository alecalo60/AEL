
using AEL.Application.Interfaces.Repositories;
using AEL.Application.Interfaces.Services;
using AEL.Domain.Entities;

namespace AEL.Application.Services
{
    public class GenericService<T> : IGenericService<T> where T : BaseEntity<int>
    {
        public readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task AddAsync(T entity) => await _repository.AddAsync(entity);

        public async Task UpdateAsync(T entity) => await _repository.UpdateAsync(entity);

        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }

}
