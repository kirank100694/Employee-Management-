using AutoMapper;
using EmployeeManagement.Caching;
using EmployeeManagement.Data;
using EmployeeManagement.Helper;
using EmployeeManagement.Models;
using LazyCache;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeManagement.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICacheProvider _cacheProvider;

        public EmployeeRepository(EmployeeDbContext context, IMapper mapper, ICacheProvider cacheProvider)
        {
            _context = context;
            _mapper = mapper;
            _cacheProvider = cacheProvider;
        }

        public async Task<List<EmployeeModel>> GetEmployees(PagingHelper pagingHelper)
        {
            var employees = _context.Employees.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(pagingHelper.name))
            {
                employees = employees.Where(m => m.Name.ToLower().Contains(pagingHelper.name) || m.Department.ToLower().Contains(pagingHelper.name));
            }

            // Sorting
            if (!string.IsNullOrEmpty(pagingHelper.sortBy))
            {
                if (pagingHelper.sortBy == "name")
                {
                    employees = pagingHelper.sortByDecending ? employees.OrderByDescending(m => m.Name) : employees.OrderBy(m => m.Name);
                }
                else if (pagingHelper.sortBy == "department")
                {
                    employees = pagingHelper.sortByDecending ? employees.OrderByDescending(m => m.Department) : employees.OrderBy(m => m.Department);
                }
            }

            // Pagination
            var totalItems = await employees.CountAsync();

            var pagedEmployees = await employees.Skip((pagingHelper.page - 1) * pagingHelper.pageSize).Take(pagingHelper.pageSize).ToListAsync();

            if (!_cacheProvider.TryGetValue(CacheKey.EmployeeModel, out List<EmployeeModel> employeeModel))
            {
                var cacheEntryOption = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 1024
                };

                _cacheProvider.Set(CacheKey.EmployeeModel, employees, cacheEntryOption);
            }

            var employeeModels = _mapper.Map<List<EmployeeModel>>(pagedEmployees);

            return employeeModels;
        }

        public async Task<EmployeeModel> GetEmployeeById(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);

            if (!_cacheProvider.TryGetValue(CacheKey.EmployeeModel, out List<EmployeeModel> employeeModel))
            {
                var cacheEntryOption = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 1024
                };
                _cacheProvider.Set(CacheKey.EmployeeModel, employee, cacheEntryOption);
            }
            return _mapper.Map<EmployeeModel>(employee);
        }

        public async Task<int> AddEmployee(EmployeeModel employeeModel)
        {
            var employee = _mapper.Map<Employee>(employeeModel);
            employeeModel.CreatedDate = DateTime.Now;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return employeeModel.Id;
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
