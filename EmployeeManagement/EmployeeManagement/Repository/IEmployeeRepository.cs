using EmployeeManagement.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeManagement.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeesById(int employeeId);
        Task<int> AddEmployeesAsync(Employee employee);
        Task<bool> IsIdExists(int Id);
        Task UpdateEmployeesAsync(int employeeId, Employee employee);
        Task UpdateEmployeesPatchAsync(int employeeId, JsonPatchDocument employee);
        Task DeleteEmployeesAsync(int employeeId);
    }
}