using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Repository
{
    public interface IAccountRepository
    {
        Task<string> Login(SignInModel signInModel);
        Task<IdentityResult> Register(SignUpModel signUpModel);
    }
}