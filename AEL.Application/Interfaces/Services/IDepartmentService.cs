using AEL.Application.DTOs;
using AEL.Domain.Entities;

namespace AEL.Application.Interfaces.Services
{
    public interface IDepartmentService : IGenericService<Department>
    {
        Task<DepartmentDTO> GetByIdAsync(int id);
        Task<IEnumerable<DepartmentDTO>> GetAllAsync();
        Task<ResponseDTO> AddAsync(CreateDepartmentDTO createCountryDTO);
        Task<ResponseDTO> UpdateAsync(UpdateDepartmentDTO updateCountryDTO);
        Task<ResponseDTO> DeleteAsync(int id);
        Task<IEnumerable<EmployeeDTO>> GetEmployeesByDepartmentAsync(int departmentId);
        Task<ResponseDTO> GetTotalSalaryByDepartmentAsync(int departmentId);

    }
}
