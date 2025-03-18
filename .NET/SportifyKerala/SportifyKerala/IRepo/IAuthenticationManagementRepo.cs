using SportifyKerala.Dto;
using SportifyKerala.Models;

namespace SportifyKerala.IRepo
{
    public interface IAuthenticationManagementRepo
    {
        public Task<Users> Login(LoginDto login);  //for login
    }
}
