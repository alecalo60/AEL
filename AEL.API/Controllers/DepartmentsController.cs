using AEL.Application.DTOs;
using AEL.Application.Interfaces.Services;
using AEL.Application.Services;
using AEL.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AEL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetAll()
        {
            var departments = await _departmentService.GetAllAsync();
            return Ok(departments);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateDepartmentDTO createDepartmentDTO)
        {
            var response = await _departmentService.AddAsync(createDepartmentDTO);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDepartmentDTO updateDepartmentDTO)
        {

            var response = await _departmentService.UpdateAsync(updateDepartmentDTO);

            return response.Success ? Ok(response) : BadRequest(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _departmentService.DeleteAsync(id);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}/employees")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployeesByDepartment(int id)
        {
            var employees = await _departmentService.GetEmployeesByDepartmentAsync(id);

            if (employees == null || !employees.Any())
            {
                return NotFound($"No employees found for department ID {id}");
            }

            return Ok(employees);
        }
        [HttpGet("{departmentId}/total-salary")]
        public async Task<IActionResult> GetTotalSalary(int departmentId)
        {
            var response = await _departmentService.GetTotalSalaryByDepartmentAsync(departmentId);

            if (response.Success)
            {
                return Ok(response);

            }
            else
            {
                return NotFound(response);
            }           
        }
    }
}
