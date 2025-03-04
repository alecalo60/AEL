using AEL.Application.DTOs;
using AEL.Application.Services;
using AEL.Domain.Entities;
using AutoMapper;
using Moq;
using Xunit;
using AEL.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AEL.Tests.Services
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IGenericRepository<Department>> _mockDepartmentRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DepartmentService _departmentService;

        public DepartmentServiceTests()
        {
            _mockDepartmentRepository = new Mock<IGenericRepository<Department>>();
            _mockMapper = new Mock<IMapper>();

            _departmentService = new DepartmentService(
                _mockDepartmentRepository.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDepartmentDTO_WhenDepartmentExists()
        {
            var department = new Department { Id = 1, Name = "HR" };
            _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(1, null))
                .ReturnsAsync(department);
            _mockMapper.Setup(m => m.Map<DepartmentDTO>(department))
                .Returns(new DepartmentDTO { Id = 1, Name = "HR" });

            var result = await _departmentService.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("HR", result.Name);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSuccess_WhenDepartmentIsCreated()
        {
            var createDepartmentDTO = new CreateDepartmentDTO { Name = "Finance" };
            var department = new Department { Id = 1, Name = "Finance" };

            _mockDepartmentRepository.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Department, bool>>>()))
                .ReturnsAsync(false);
            _mockMapper.Setup(m => m.Map<Department>(createDepartmentDTO))
                .Returns(department);
            _mockDepartmentRepository.Setup(repo => repo.AddAsync(department))
                .Returns(Task.CompletedTask);

            var result = await _departmentService.AddAsync(createDepartmentDTO);

            Assert.True(result.Success);
            Assert.Equal("Department created successfully", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccess_WhenDepartmentIsUpdated()
        {
            var updateDepartmentDTO = new UpdateDepartmentDTO { Id = 1, Name = "Updated HR" };
            var department = new Department { Id = 1, Name = "Updated HR" };

            _mockMapper.Setup(m => m.Map<Department>(updateDepartmentDTO))
                .Returns(department);
            _mockDepartmentRepository.Setup(repo => repo.UpdateAsync(department))
                .Returns(Task.CompletedTask);

            var result = await _departmentService.UpdateAsync(updateDepartmentDTO);

            Assert.True(result.Success);
            Assert.Equal("Department updated successfully", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccess_WhenDepartmentIsDeleted()
        {
            _mockDepartmentRepository.Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            var result = await _departmentService.DeleteAsync(1);

            Assert.True(result.Success);
            Assert.Equal("Department deleted successfully", result.Message);
        }

        [Fact]
        public async Task GetEmployeesByDepartmentAsync_ShouldReturnEmployees_WhenDepartmentExists()
        {
            var department = new Department
            {
                Id = 1,
                Name = "HR",
                Employees = new List<Employee>
                {
                    new Employee { Id = 1, Name = "Alice" },
                    new Employee { Id = 2, Name = "Bob" }
                }
            };

            _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(1, It.IsAny<Func<IQueryable<Department>, IIncludableQueryable<Department, object>>?>()))
                .ReturnsAsync(department);
            _mockMapper.Setup(m => m.Map<IEnumerable<EmployeeDTO>>(department.Employees))
                .Returns(new List<EmployeeDTO>
                {
                    new EmployeeDTO { Id = 1, Name = "Alice" },
                    new EmployeeDTO { Id = 2, Name = "Bob" }
                });

            var result = await _departmentService.GetEmployeesByDepartmentAsync(1);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
