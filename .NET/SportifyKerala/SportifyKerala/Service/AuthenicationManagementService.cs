using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using SportifyKerala.Dto;
using SportifyKerala.IRepo;
using SportifyKerala.IService;
using SportifyKerala.Utilitys;

namespace SportifyKerala.Service
{
    public class AuthenicationManagementService : IAuthenticationManagementService
    {
        private readonly IAuthenticationManagementRepo _repo;
        private readonly IConfiguration _configuration;
        public AuthenicationManagementService(IAuthenticationManagementRepo repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }


        //for login
        public async Task<APIResponse<string>> Login(LoginDto login)
        {
            var result = await _repo.Login(login);
            if(result == null)
            {
                return APIResponse<string>.Error("Invalid Email id");
            }
            var verifyPassword = forPasswordHashing.VerifyPassword(login.passwordOfUser,result.Password,result.salt);
            if(!verifyPassword)
            {
                return APIResponse<string>.Error("Invalid Password");
            }
            var masked = new UserToListDto
            {
                idOfUser = result.UserId,
                nameOfUser = result.FullName,
                emailOfUser = result.Email,
                phoneOfUser = result.Phone,
                addressOfUser = result.Address,
                idOfDistrict = result.DistrictId,
                commitie = result.CommitieVerfied,
            };

            var tokenGenerator = new Token(_configuration);
            var accessToken = tokenGenerator.GenerateToken(masked);

            if(accessToken == null)
            {
                return APIResponse<string>.Error("Failed to generate Token");
            }
            return APIResponse<string>.Success(accessToken, "success");
        }

    }
}
