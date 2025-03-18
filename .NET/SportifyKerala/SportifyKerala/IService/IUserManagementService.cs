using SportifyKerala.Dto;
using SportifyKerala.Models;
using SportifyKerala.Utilitys;


namespace SportifyKerala.IService
{
    public interface IUserManagementService
    {
        public Task<APIResponse<bool>> UserRegistration(UserForCreationDto user);  //For Users Registration
        public Task<APIResponse<bool>> ClubRegistration(UserForCreationDto user);  //For Clubs Registration (data will be only verified after otp is entered correctly)
        public Task<APIResponse<bool>> VerifyOtp(string otp); //to verify the otp


        public Task<APIResponse<UserToListDto>> GetOwnProfile(Guid uid); //For gettig own data

    }
}
