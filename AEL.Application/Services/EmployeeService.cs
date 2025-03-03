using AEL.Application.DTOs;
using AEL.Application.Interfaces.Repositories;
using AEL.Application.Interfaces.Services;
using AEL.Application.Services;
using AEL.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AEL.Application.Services
{
    public class EmployeeService : GenericService<Employee>, IEmployeeService
    {
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IMapper _mapper;
        private IMapper @object;

        public EmployeeService(
        IGenericRepository<Employee> employeeRepository,
        IGenericRepository<Department> departmentRepository,
        IMapper mapper)
        : base(employeeRepository) 
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public EmployeeService(IGenericRepository<Employee> repository, IMapper @object) : base(repository)
        {
            this.@object = @object;
        }

        public async Task<EmployeeDTO> GetByIdAsync(int id)
        {
            var employee = await _repository.Query()
                .Where(e => e.Id == id)
                .Select(e => new Employee
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Salary = e.Salary,
                    DepartmentId = e.DepartmentId,
                    Department = new Department
                    {
                        Id = e.Department.Id,
                        Name = e.Department.Name,
                        CreatedAt = e.Department.CreatedAt,
                        UpdatedAt = e.Department.UpdatedAt
                    }
                })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return null;
            }

            return _mapper.Map<EmployeeDTO>(employee);
        }


        public async Task<IEnumerable<EmployeeDTO>> GetAllAsync()
        {
            var employees = await _repository.Query()
                .Select(e => new EmployeeDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Salary = e.Salary,
                    Position = e.Position,
                    DepartmentId = e.DepartmentId,
                    Department = e.Department != null ? new Department
                    {
                        Id = e.Department.Id,
                        Name = e.Department.Name,
                        CreatedAt = e.Department.CreatedAt,
                        UpdatedAt = e.Department.UpdatedAt
                    } : null
                })
                .ToListAsync();

            return employees;
        }


        public async Task<ResponseDTO> AddAsync(CreateEmployeeDTO createEmployeeDTO)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(createEmployeeDTO.DepartmentId);
                if (department == null)
                {
                    return new ResponseDTO
                    {
                        Success = false,
                        Message = $"The DepartmentId {createEmployeeDTO.DepartmentId} doesn't exist."
                    };
                }

                var employeeExists = await _repository.ExistsAsync(e => e.Email == createEmployeeDTO.Email);
                if (employeeExists)
                {
                    return new ResponseDTO
                    {
                        Success = false,
                        Message = "The employee already exists in the database."
                    };
                }
                var employee = _mapper.Map<Employee>(createEmployeeDTO);

                await _repository.AddAsync(employee);

                return new ResponseDTO
                {
                    Success = true,
                    Message = "Employee created successfully",
                    Data = createEmployeeDTO
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Success = false,
                    Message = $"Error creating employee: {ex.Message}"
                };
            }
        }

        public async Task<ResponseDTO> UpdateAsync(UpdateEmployeeDTO updateEmployeeDTO)
        {
            try
            {
                var employee = _mapper.Map<Employee>(updateEmployeeDTO);
                await _repository.UpdateAsync(employee);

                return new ResponseDTO
                {
                    Success = true,
                    Message = "Employee updated successfully",
                    Data = updateEmployeeDTO
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating employee: {ex.Message}", ex);
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
                    Message = "Employee deleted successfully",
                    Data = id
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting employee: {ex.Message}", ex);
            }
        }
    }
}
