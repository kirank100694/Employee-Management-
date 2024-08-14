using AutoMapper;
using EmployeeManagement.Data;

namespace EmployeeManagement.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Employee, EmployeeModel>().ReverseMap();
        }
    }
}
