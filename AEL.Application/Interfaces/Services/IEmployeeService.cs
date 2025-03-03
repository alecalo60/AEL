using AEL.Application.DTOs;
using AEL.Domain.Entities;

namespace AEL.Application.Interfaces.Services
{
    public interface IEmployeeService : IGenericService<Employee>
    {
        Task<EmployeeDTO> GetByIdAsync(int id);
        Task<IEnumerable<EmployeeDTO>> GetAllAsync();
        Task<ResponseDTO> AddAsync(CreateEmployeeDTO createCountryDTO);
        Task<ResponseDTO> UpdateAsync(UpdateEmployeeDTO updateCountryDTO);
        Task<ResponseDTO> DeleteAsync(int id);
    }
}
