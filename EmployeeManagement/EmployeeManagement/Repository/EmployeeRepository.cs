using AutoMapper;
using EmployeeManagement.Data;
using EmployeeManagement.Migrations;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;

        public EmployeeRepository(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeContext.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _employeeContext.Employees.FindAsync(id);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            _employeeContext.Employees.Add(employee);
            await _employeeContext.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _employeeContext.Entry(employee).State = EntityState.Modified;
            await _employeeContext.SaveChangesAsync();
        }

        public async Task UpdateEmployeesPatchAsync(int employeeId, JsonPatchDocument<Employee> patchEmployee)
        {
            var employee = await _employeeContext.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            // Apply the patch document to the employee entity
            patchEmployee.ApplyTo(employee);

            // Update the employee's updated date
            employee.UpdatedDate = DateTime.UtcNow;

            // Mark the entity as modified
            _employeeContext.Entry(employee).State = EntityState.Modified;

            // Save changes to the database
            await _employeeContext.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeContext.Employees.FindAsync(id);
            if (employee != null)
            {
                _employeeContext.Employees.Remove(employee);
                await _employeeContext.SaveChangesAsync();
            }
        }
    }
}
