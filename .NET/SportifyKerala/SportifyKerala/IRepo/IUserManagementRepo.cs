using SportifyKerala.Dto;
using SportifyKerala.Models;
using SportifyKerala.Utilitys;

namespace SportifyKerala.IRepo
{
    public interface IUserManagementRepo
    {
        public Task<int> UserRegistration(UserForCreationDto user,string salt);  //for Register as a User
        public Task<Users> ClubRegistration(UserForCreationDto user, string salt);  //For Clubs Registration (data will be only verified after otp is entered correctly)
                public Task<int> SaveOtp(string otp_data,Guid userId,DateTime Expiry);  //when the user details is entered the otp needs to store in the otp table, and then this otptableis used to check and then verfiy the club,
        public Task<Users> VerifyOtp(string otp); //to verify the otp
                public Task<int> VerifyingUseRemovingOtp(Guid id); //if the otp is correct we need to approve the user and delete the otp form th eotp table



        public Task<APIResponse<IEnumerable<Districts>>> GetAllDistricts(); //for get all districts


        public Task<Users> GetOwnProfile(Guid uid); //For gettig own data

    }
}
