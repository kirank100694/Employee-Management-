using EmployeeManagement.Helper;
using EmployeeManagement.Models;
using EmployeeManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromBody] PagingHelper pagingHelper)
        {
            var employees = await _employeeRepository.GetEmployees(pagingHelper);

            if (employees != null && employees.Count == 0)
            {
                return BadRequest("No employees found.");
            }

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetEmployeeById([FromRoute] int id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);

            if (employee == null)
            {
                return BadRequest($"Employee with ID {id} not found.");
            }

            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult> AddEmployee([FromBody] EmployeeModel employeeModel)
        {
            if (await _employeeRepository.IsEmployeeExists(employeeModel.Email))
            {
                return BadRequest("Employee is already exists.");
            }
            var id = await _employeeRepository.AddEmployee(employeeModel);

            return CreatedAtAction(nameof(GetEmployeeById), new { id = id, Controller = "employee" }, id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee([FromBody] EmployeeModel employeeModel, [FromRoute] int id)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeById(id);

            if (existingEmployee == null)
            {
                return BadRequest($"Employee Id {id} is not found.");
            }

            await _employeeRepository.UpdateEmployee(existingEmployee, employeeModel);

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateEmployee([FromBody] JsonPatchDocument employeeModel, [FromRoute] int id)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeById(id);
            if (existingEmployee == null)
            {
                return BadRequest($"Employee Id {id} is not found.");
            }

            await _employeeRepository.UpdateEmployee(existingEmployee, employeeModel);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {

            if (!await _employeeRepository.IsEmployeeExists(id))
            {
                return BadRequest($"Employee Id {id} is not found.");
            }

            await _employeeRepository.DeleteEmployeeById(id);
            return Ok();
        }
    }
}
