using AEL.Application.DTOs;
using AEL.Application.Services;
using AEL.Domain.Entities;
using AEL.Infrastructure.Persistence.Repositories;
using AEL.Infrastructure.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using AEL.Application.Interfaces.Repositories;
using AEL.Application.Interfaces.Services;

namespace AEL.Tests.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IGenericRepository<Employee>> _mockEmployeeRepository;
        private readonly Mock<IGenericRepository<Department>> _mockDepartmentRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _mockEmployeeRepository = new Mock<IGenericRepository<Employee>>();
            _mockDepartmentRepository = new Mock<IGenericRepository<Department>>();
            _mockMapper = new Mock<IMapper>();

            _employeeService = new EmployeeService(
                _mockEmployeeRepository.Object,
                _mockDepartmentRepository.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEmployeeDTO_WhenEmployeeExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var employee = new Employee
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john@example.com",
                    Salary = 5000,
                    DepartmentId = 1,
                    Department = new Department { Id = 1, Name = "IT" }
                };

                context.Employees.Add(employee);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new GenericRepository<Employee>(context);

                _mockMapper.Setup(m => m.Map<EmployeeDTO>(It.IsAny<Employee>()))
                    .Returns((Employee emp) => new EmployeeDTO
                    {
                        Id = emp.Id,
                        Name = emp.Name,
                        Email = emp.Email,
                        Salary = emp.Salary,
                        DepartmentId = emp.DepartmentId,
                        Department = new Department
                        {
                            Id = emp.Department.Id,
                            Name = emp.Department.Name
                        }
                    });

                var employeeService = new EmployeeService(repository, _mockDepartmentRepository.Object, _mockMapper.Object);

                // Act
                var result = await employeeService.GetByIdAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("John Doe", result.Name);
            }
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenEmployeeIsCreated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {

                context.Departments.RemoveRange(context.Departments);
                context.Employees.RemoveRange(context.Employees);
                await context.SaveChangesAsync();


                var department = new Department { Id = 1, Name = "IT" };
                context.Departments.Add(department);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var employeeRepository = new GenericRepository<Employee>(context);
                var departmentRepository = new GenericRepository<Department>(context);

                var mockMapper = new Mock<IMapper>();
                mockMapper.Setup(m => m.Map<Employee>(It.IsAny<CreateEmployeeDTO>()))
                    .Returns((CreateEmployeeDTO dto) => new Employee
                    {
                        Id = 1,
                        Name = dto.Name,
                        Email = dto.Email,
                        Salary = dto.Salary,
                        DepartmentId = dto.DepartmentId
                    });

                var employeeService = new EmployeeService(employeeRepository, departmentRepository, mockMapper.Object);

                var createEmployeeDTO = new CreateEmployeeDTO
                {
                    Name = "Alice",
                    Email = "alice@example.com",
                    Salary = 4500,
                    DepartmentId = 1
                };

                // Act
                var result = await employeeService.AddAsync(createEmployeeDTO);

                // Assert
                Assert.True(result.Success); // Asegúrate de que Success sea true
                Assert.Equal("Employee created successfully", result.Message);
            }
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfEmployeeDTOs()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Employees.RemoveRange(context.Employees);
                context.Departments.RemoveRange(context.Departments);
                await context.SaveChangesAsync();

                var department = new Department { Id = 1, Name = "IT" };
                var employee1 = new Employee { Id = 1, Name = "John Doe", Email = "john@example.com", Salary = 5000, DepartmentId = 1, Department = department };
                var employee2 = new Employee { Id = 2, Name = "Jane Doe", Email = "jane@example.com", Salary = 6000, DepartmentId = 1, Department = department };

                context.Departments.Add(department);
                context.Employees.AddRange(employee1, employee2);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new GenericRepository<Employee>(context);
                var departmentRepository = new GenericRepository<Department>(context);

                _mockMapper.Setup(m => m.Map<EmployeeDTO>(It.IsAny<Employee>()))
                    .Returns((Employee emp) => new EmployeeDTO
                    {
                        Id = emp.Id,
                        Name = emp.Name,
                        Email = emp.Email,
                        Salary = emp.Salary,
                        DepartmentId = emp.DepartmentId,
                        Department = new Department
                        {
                            Id = emp.Department.Id,
                            Name = emp.Department.Name
                        }
                    });

                var employeeService = new EmployeeService(repository, departmentRepository, _mockMapper.Object);

                // Act
                var result = await employeeService.GetAllAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
            }
        }
        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccess_WhenEmployeeIsUpdated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Employees.RemoveRange(context.Employees);
                context.Departments.RemoveRange(context.Departments);
                await context.SaveChangesAsync();

                var department = new Department { Id = 1, Name = "IT" };
                var employee = new Employee { Id = 1, Name = "John Doe", Email = "john@example.com", Salary = 5000, DepartmentId = 1, Department = department };

                context.Departments.Add(department);
                context.Employees.Add(employee);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new GenericRepository<Employee>(context);
                var departmentRepository = new GenericRepository<Department>(context);

                var mockMapper = new Mock<IMapper>();
                mockMapper.Setup(m => m.Map<Employee>(It.IsAny<UpdateEmployeeDTO>()))
                    .Returns((UpdateEmployeeDTO dto) => new Employee
                    {
                        Id = dto.Id,
                        Name = dto.Name,
                        Email = dto.Email,
                        Salary = dto.Salary,
                        DepartmentId = dto.DepartmentId
                    });

                var employeeService = new EmployeeService(repository, departmentRepository, mockMapper.Object);

                var updateEmployeeDTO = new UpdateEmployeeDTO
                {
                    Id = 1,
                    Name = "John Updated",
                    Email = "john.updated@example.com",
                    Salary = 5500,
                    DepartmentId = 1
                };

                // Act
                var result = await employeeService.UpdateAsync(updateEmployeeDTO);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("Employee updated successfully", result.Message);
            }
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccess_WhenEmployeeIsDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Employees.RemoveRange(context.Employees);
                context.Departments.RemoveRange(context.Departments);
                await context.SaveChangesAsync();

                var department = new Department { Id = 1, Name = "IT" };
                var employee = new Employee { Id = 1, Name = "John Doe", Email = "john@example.com", Salary = 5000, DepartmentId = 1, Department = department };

                context.Departments.Add(department);
                context.Employees.Add(employee);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new GenericRepository<Employee>(context);
                var departmentRepository = new GenericRepository<Department>(context);

                var employeeService = new EmployeeService(repository, departmentRepository, _mockMapper.Object);

                // Act
                var result = await employeeService.DeleteAsync(1);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("Employee deleted successfully", result.Message);
            }
        }
    }
}