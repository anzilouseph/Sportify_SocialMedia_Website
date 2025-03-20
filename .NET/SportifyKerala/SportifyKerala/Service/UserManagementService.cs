using Microsoft.Identity.Client;
using SportifyKerala.Dto;
using SportifyKerala.IRepo;
using SportifyKerala.IService;
using SportifyKerala.Models;
using SportifyKerala.Utilitys;

namespace SportifyKerala.Service
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserManagementRepo _repo;
        public UserManagementService(IUserManagementRepo repo)
        {
            _repo = repo;
        }

        //For Users Registration
        public async Task<APIResponse<bool>> UserRegistration(UserForCreationDto user)
        {
            user.passwordOfUser = forPasswordHashing.HashPassword(user.passwordOfUser, out string salt);
            var rowAffected = await _repo.UserRegistration(user,salt);
            if(rowAffected ==0)
            {
                return APIResponse<bool>.Error("Unable to add User");
            }

            var sentMail = new MailHelper();
            sentMail.SendAccountCreationEmail(user.nameOfUser,user.emailOfUser);
            return APIResponse<bool>.Success(true, "User Added Sucessfully");
        }


        //For Clubs Registration
        public async Task<APIResponse<bool>> ClubRegistration(UserForCreationDto user)
        {
            user.passwordOfUser = forPasswordHashing.HashPassword(user.passwordOfUser, out string salt);
            var result = await _repo.ClubRegistration(user, salt);
            if (result == null)
            {
                return APIResponse<bool>.Error("Unable to add User");
            }
            var otp = new Random().Next(1000, 9999).ToString();
            var expiry = DateTime.Now.AddMinutes(10);

            var rowAffected = await _repo.SaveOtp(otp, result.UserId, expiry);  //here we calling the fn to save the otp,and that otp woll be expired in 10 ms
            if (rowAffected == 0)
            {
                return APIResponse<bool>.Error("Unable to add Otp");
            }

            var sentEmail = new MailHelper();
            sentEmail.SendOtpEmail(result.FullName, result.Email, otp);   //here we are sending the mail that cobtains the otp to the user
            return APIResponse<bool>.Success(true, "OTP Send");
        }
        //to verify the otp
        public async Task<APIResponse<bool>> VerifyOtp(string otp)
        {
            var result = await _repo.VerifyOtp(otp);
            if (result == null)
            {
                return APIResponse<bool>.Error("wrong pin");
            }

            var rowAffected = await _repo.VerifyingUseRemovingOtp(result.UserId);
            if (rowAffected == 0)
            {
                return APIResponse<bool>.Error("Unable to Verify User");
            }
            return APIResponse<bool>.Success(true, "Verified");
        }




        //For gettig own data
        public async Task<APIResponse<UserToListDto>> GetOwnProfile(Guid uid)
        {
            var result = await _repo.GetOwnProfile(uid);
            if(result == null)
            {
                return APIResponse<UserToListDto>.Error("No User in this id");
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
                roleOfUser = result.Role,
                imageName = result.ProfileImage,
                
            };
            return APIResponse<UserToListDto>.Success(masked, "Success");
        }


    }
}
