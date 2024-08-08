using AutoMapper;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly IMapper _mapper;

        public EmployeeRepository(EmployeeContext employeeContext, IMapper mapper)
        {
            _employeeContext = employeeContext;
            _mapper = mapper;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            var records = await _employeeContext.Employees.ToListAsync();
            return _mapper.Map<List<Employee>>(records);
        }

        public async Task<Employee> GetEmployeesById(int employeeId)
        {
            var records = await _employeeContext.Employees.FindAsync(employeeId);
            return _mapper.Map<Employee>(records);
        }

        public async Task<bool> IsIdExists(int Id)
        {
            var records = await _employeeContext.Employees.FindAsync(Id);
            return records != null ? true : false;
        }

        public async Task<int> AddEmployeesAsync(Employee employee)
        {
            var records = _mapper.Map<Employee>(employee);
            records.Createddate = DateTime.Now;

            _employeeContext.Employees.Add(employee);
            await _employeeContext.SaveChangesAsync();

            return records.Id;

        }

        public async Task UpdateEmployeesAsync(int employeeId, Employee employee)
        {
            var records = _mapper.Map<Employee>(employee);
            records.Createddate = DateTime.Now;

            _employeeContext.Employees.Update(records);
            await _employeeContext.SaveChangesAsync();
        }

        public async Task UpdateEmployeesPatchAsync(int employeeId, JsonPatchDocument employee)
        {
            var record = await _employeeContext.Employees.FindAsync(employeeId);
            if (record != null)
            {
                employee.ApplyTo(record);
                await _employeeContext.SaveChangesAsync();
            }
        }

        public async Task DeleteEmployeesAsync(int employeeId)
        {
            var records = new Employee()
            {
                Id = employeeId
            };

            _employeeContext.Employees.Remove(records);

            await _employeeContext.SaveChangesAsync();
        }
    }
}
