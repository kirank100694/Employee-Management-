using EmployeeManagement.Models;
using EmployeeManagement.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees();
            if (employees.Count == 0)
            {
                return NotFound("Employees are not available");
            }
            else
            {
                return Ok(employees);

            }

        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetEmployeesById([FromRoute] int Id)
        {
            var employee = await _employeeRepository.GetEmployeesById(Id);
            if (employee == null)
            {
                return NotFound($"Employee with Id {Id} not found");
            }

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployees([FromBody] Employee employee)
        {
            if (await _employeeRepository.IsIdExists(employee.Id))
            {
                return BadRequest("Employeed Id already exist");
            }
            var id = await _employeeRepository.AddEmployeesAsync(employee);

            return CreatedAtAction(nameof(GetEmployeesById), new { id = employee.Id, controller = "employee" }, employee);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateEmployees([FromBody] Employee employee, [FromRoute] int Id)
        {
            if (await _employeeRepository.IsIdExists(Id))
            {
                await _employeeRepository.UpdateEmployeesAsync(Id, employee);
                return Ok();
            }
            else
            {
                return NotFound("Employee Id is notfound");
            }
        }

        [HttpPatch("{Id}")]
        public async Task<IActionResult> UpdateEmployeesPatch([FromBody] JsonPatchDocument employee, [FromRoute] int Id)
        {
            if (await _employeeRepository.IsIdExists(Id))
            {
                await _employeeRepository.UpdateEmployeesPatchAsync(Id, employee);

                return Ok();
            }
            else
            {
                return NotFound("Selected Id is not Exist");
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteEmployees([FromRoute] int Id)
        {
            if (await _employeeRepository.IsIdExists(Id))
            {
                await _employeeRepository.DeleteEmployeesAsync(Id);

                return Ok();
            }
            else
            {
                return NotFound("Selected Id is notfound");
            }
        }
    }
}
