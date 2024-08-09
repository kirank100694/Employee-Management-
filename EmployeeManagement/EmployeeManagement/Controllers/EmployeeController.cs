using AutoMapper;
using EmployeeManagement.Data;
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
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Employee>> GetEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            if (employees.Count == 0)
            {
                return NotFound("No employee exist");
            }

            var employeeDate = _mapper.Map<List<EmployeeCreateDate>>(employees);
            return Ok(employeeDate);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeCreateDate>> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            var employeeDto = _mapper.Map<EmployeeCreateDate>(employee);
            return Ok(employeeDto);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeCreateDate>> PostEmployee(Employee employeeDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(employeeDate.Id);
            if (existingEmployee != null)
            {
                return Conflict("Employee with this ID already exists.");
            }

            var employee = _mapper.Map<Employee>(employeeDate);
            employee.CreatedDate = DateTime.UtcNow;
            employee.UpdatedDate = DateTime.UtcNow;

            await _employeeRepository.AddEmployeeAsync(employee);
            var newEmployeeDate = _mapper.Map<EmployeeCreateDate>(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, newEmployeeDate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeUpdateDate employeeDate)
        {
            if (id != employeeDate.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            var employee = _mapper.Map<Employee>(employeeDate);
            employee.CreatedDate = existingEmployee.CreatedDate;  // Preserve original CreatedDate
            employee.UpdatedDate = DateTime.UtcNow;

            await _employeeRepository.UpdateEmployeeAsync(employee);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmployee(int id, JsonPatchDocument<EmployeeUpdateDate> employeePatch)
        {
            if (employeePatch == null)
            {
                return BadRequest();
            }

            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            var employeeDate = _mapper.Map<EmployeeUpdateDate>(existingEmployee);
            employeePatch.ApplyTo(employeeDate, ModelState);

            if (!TryValidateModel(employeeDate))
            {
                return ValidationProblem(ModelState);
            }

            var employee = _mapper.Map<Employee>(employeeDate);
            employee.CreatedDate = existingEmployee.CreatedDate;  
            employee.UpdatedDate = DateTime.UtcNow;

            await _employeeRepository.UpdateEmployeeAsync(employee);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteEmployeeAsync(id);
            return NoContent();
        }
    }
}
