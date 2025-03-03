using AEL.Application.DTOs;
using AEL.Application.Interfaces.Services;
using AEL.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AEL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAll()
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateEmployeeDTO createEmployeeDTO)
        {
            var response = await _employeeService.AddAsync(createEmployeeDTO);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeDTO updateEmployeeDTO)
        {

            var response = await _employeeService.UpdateAsync(updateEmployeeDTO);

            return response.Success ? Ok(response) : BadRequest(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _employeeService.DeleteAsync(id);

            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
