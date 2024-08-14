using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeManagement.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeModel>> GetEmployees();

        Task<EmployeeModel> GetEmployeeById(int employeeId);

        Task<int> AddEmployee(EmployeeModel employeeModel);

        Task UpdateEmployee(EmployeeModel existingEmployee, EmployeeModel employeeModel);

        Task UpdateEmployee(EmployeeModel existingEmployee, JsonPatchDocument employeeModel);

        Task DeleteEmployeeById(int employeeId);

        Task<bool> IsEmployeeExists(int id);

        Task<bool> IsEmployeeExists(string email);
    }
}