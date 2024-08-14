using AutoMapper;
using EmployeeManagement.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeRepository(EmployeeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EmployeeModel>> GetEmployees()
        {
            var employee = await _context.Employees.ToListAsync();
            return _mapper.Map<List<EmployeeModel>>(employee);
        }

        public async Task<EmployeeModel> GetEmployeeById(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            return _mapper.Map<EmployeeModel>(employee);
        }

        public async Task<int> AddEmployee(EmployeeModel employeeModel)
        {
            var employee = _mapper.Map<Employee>(employeeModel);
            employee.CreatedDate = DateTime.Now;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return employee.Id;
        }

        public async Task UpdateEmployee(EmployeeModel existingEmployee, EmployeeModel employeeModel)
        {
            _mapper.Map(employeeModel, existingEmployee);

            existingEmployee.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployee(EmployeeModel existingEmployee, JsonPatchDocument employeeModel)
        {
            employeeModel.ApplyTo(existingEmployee);

            existingEmployee.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeById(int employeeId)
        {
            var employee = new Employee() { Id = employeeId };

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsEmployeeExists(int id)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> IsEmployeeExists(string email)
        {
            return await _context.Employees.AnyAsync(e => e.Email == email);
        }
    }
}
