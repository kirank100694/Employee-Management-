using EmployeeManagement.Data;
using EmployeeManagement.Models;
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

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _employeeContext.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _employeeContext.Employees.FindAsync(id);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            _employeeContext.Add(employee);
            await _employeeContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _employeeContext.Employees.FindAsync(employee.Id);
            if (existingEmployee == null)
            {
                return null;
            }

            _employeeContext.Entry(existingEmployee).CurrentValues.SetValues(employee);
            existingEmployee.UpdatedDate = DateTime.UtcNow;

            await _employeeContext.SaveChangesAsync();
            return existingEmployee;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return false;
            }

            _employeeContext.Employees.Remove(employee);
            await _employeeContext.SaveChangesAsync();
            return true;
        }
    }
}
