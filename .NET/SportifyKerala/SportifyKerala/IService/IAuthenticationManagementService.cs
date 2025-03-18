using SportifyKerala.Dto;
using SportifyKerala.Models;
using SportifyKerala.Utilitys;

namespace SportifyKerala.IService
{
    public interface IAuthenticationManagementService
    {        
        public Task<APIResponse<string>> Login(LoginDto login); //for login
    }
}
