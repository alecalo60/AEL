using AEL.Application.DTOs;
using AEL.Application.Interfaces.Repositories;
using AEL.Application.Interfaces.Services;
using AEL.Application.Services;
using AEL.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AEL.Application.Services
{
    public class DepartmentService : GenericService<Department>, IDepartmentService
    {
        private readonly IMapper _mapper;

        public DepartmentService(IGenericRepository<Department> repository, IMapper mapper)
            : base(repository) 
        {
            _mapper = mapper;
        }

        public async Task<DepartmentDTO> GetByIdAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);
            return _mapper.Map<DepartmentDTO>(department);
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllAsync()
        {
            var departments = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DepartmentDTO>>(departments);
        }

        public async Task<ResponseDTO> AddAsync(CreateDepartmentDTO createDepartmentDTO)
        {
            try
            {
                var departmentExists = await _repository.ExistsAsync(c => c.Name == createDepartmentDTO.Name);
                if (departmentExists)
                {
                    throw new InvalidOperationException("The department already exists in the database.");
                }
                var department = _mapper.Map<Department>(createDepartmentDTO);
                await _repository.AddAsync(department);

                return new ResponseDTO
                {
                    Success = true,
                    Message = "Department created successfully",
                    Data = createDepartmentDTO
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating department: {ex.Message}", ex);
            }
        }

        public async Task<ResponseDTO> UpdateAsync(UpdateDepartmentDTO updateDepartmentDTO)
        {
            try
            {
                var department = _mapper.Map<Department>(updateDepartmentDTO);
                await _repository.UpdateAsync(department);

                return new ResponseDTO
                {
                    Success = true,
                    Message = "Department updated successfully",
                    Data = updateDepartmentDTO
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating department: {ex.Message}", ex);
            }
        }

        public async Task<ResponseDTO> DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);

                return new ResponseDTO
                {
                    Success = true,
                    Message = "Department deleted successfully",
                    Data = id
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting department: {ex.Message}", ex);
            }
        }
        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            var department = await _repository.GetByIdAsync(departmentId, include: d => d.Include(d => d.Employees));

            if (department == null)
            {
                return new List<EmployeeDTO>();
            }

            return _mapper.Map<IEnumerable<EmployeeDTO>>(department.Employees);
        }
        public async Task<ResponseDTO> GetTotalSalaryByDepartmentAsync(int departmentId)
        {
            var department = await _repository.GetByIdAsync(
                departmentId,
                include: d => d.Include(d => d.Employees)
            );

            var totalSalaryResponseDTO = new TotalSalaryResponseDTO();
            var response = new ResponseDTO();

            if (department == null)
            {
                totalSalaryResponseDTO.DepartmentId = departmentId;
                totalSalaryResponseDTO.DepartmentName = "Not Found";
                totalSalaryResponseDTO.TotalSalary = 0;
                response.Success = false;
                response.Message = "Department not found";
            }

            decimal totalSalary = 0;

            if (department != null)
            {
                foreach (var employee in department.Employees)
                {
                    employee.CalculateSalary(employee.Salary);
                    totalSalary += employee.Salary;
                }

                totalSalaryResponseDTO.DepartmentId = department.Id;
                totalSalaryResponseDTO.DepartmentName = department.Name;
                totalSalaryResponseDTO.TotalSalary = totalSalary;
                response.Success = true;
                response.Data = totalSalaryResponseDTO;
            }
            return response;
        }

    }
}
