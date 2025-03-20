using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Hosting;
using SportifyKerala.Context;
using SportifyKerala.Dto;
using SportifyKerala.IRepo;
using SportifyKerala.Models;
using SportifyKerala.Utilitys;
using static Mysqlx.Expect.Open.Types.Condition.Types;

namespace SportifyKerala.Repo
{
    public class UserManagementRepo : IUserManagementRepo
    {
        private readonly DapperContext _context;
        private readonly IWebHostEnvironment _env;
        public UserManagementRepo (DapperContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        //for User Registration
        public async Task<int> UserRegistration(UserForCreationDto user, string salt)
        {
            var query = "UserRegistration";
            var parameters = new DynamicParameters();
            parameters.Add("nameOfUser", user.nameOfUser,DbType.String ,ParameterDirection.Input);
            parameters.Add("emailOfUser", user.emailOfUser, DbType.String, ParameterDirection.Input);
            parameters.Add("phoneOfUser", user.phoneOfUser, DbType.String, ParameterDirection.Input);
            parameters.Add("addressOfUser", user.addressOfUser, DbType.String, ParameterDirection.Input);
            parameters.Add("idOfDistrict", user.idOfDistrict, DbType.Guid, ParameterDirection.Input);
            parameters.Add("passwordOfUser", user.passwordOfUser, DbType.String, ParameterDirection.Input);
            parameters.Add("salt", salt, DbType.String, ParameterDirection.Input);

            using(var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query,parameters,commandType:CommandType.StoredProcedure);
                connection.Close();
                return result;
            }
        }


        //for Register as a Club
        public async Task<Users> ClubRegistration(UserForCreationDto user, string salt)
        {
            var checkingQuery = "select * from Users where Email=@emailOfUser and CommitieVerfied=0";
            var query = "insert into Users(FullName,Email,Phone,Address,DistrictId,Password,salt) values (@nameOfUser,@emailOfUser,@phoneOfUser,@addressOfUser,@idOfDistrict,@passwordOfUser,@salt);select * from Users where Email=@emailOfUser;";
            var parameters = new DynamicParameters();
            parameters.Add("nameOfUser", user.nameOfUser);
            parameters.Add("emailOfUser", user.emailOfUser);
            parameters.Add("phoneOfUser", user.phoneOfUser);
            parameters.Add("addressOfUser", user.addressOfUser);
            parameters.Add("idOfDistrict", user.idOfDistrict);
            parameters.Add("passwordOfUser", user.passwordOfUser);
            parameters.Add("salt", salt);
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var checking = await connection.QueryFirstOrDefaultAsync<Users>(checkingQuery, parameters);
                connection.Close();
                if (checking!=null)
                {
                    var userId = checking.UserId;
                    var deletingQuery = "delete from OTP where UserId=@id;delete from Users where UserId=@id;";
                    var deleting = await connection.ExecuteAsync(deletingQuery, new {id=userId});
                }
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<Users>(query, parameters);
                connection.Close();
                return result;
            }
        }

        //when the user details is entered the otp needs to store in the otp table, and then this otptableis used to check and then verfiy the club
        public async Task<int> SaveOtp(string otp_data, Guid userId, DateTime Expiry)
        {
            var query = "insert into OTP (otp_data,UserId,Expiry) values (@otp_data,@UserId,@Expiry)";
            var parameters = new DynamicParameters();
            parameters.Add("otp_data", otp_data);
            parameters.Add("UserId", userId);
            parameters.Add("Expiry", Expiry);
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var rowAffected = await connection.ExecuteAsync(query, parameters);
                connection.Close();
                return rowAffected;
            }
        }

        //to verify the otp
        public async Task<Users> VerifyOtp(string otp)
        {
            var query = "select u.* from Users u inner join OTP o on u.UserId = o.UserId where o.otp_data=@otp and o.Expiry>NOW()";
            var parameters = new DynamicParameters();
            parameters.Add("otp", otp);
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<Users>(query, parameters);
                connection.Close();
                return result;
            }
        }

        //if the otp is correct we need to approve the user and delete the otp form the otp table
        public async Task<int> VerifyingUseRemovingOtp(Guid id)
        {
            var updateQuery = "update users set CommitieVerfied=1 where UserId=@id;Delete from OTP where UserId=@id";
            var parameters2 = new DynamicParameters();
            parameters2.Add("id", id);
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var rowAffected = await connection.ExecuteAsync(updateQuery, parameters2);
                connection.Close();
                return rowAffected;
            }
        }


        //For gettig own data
        public async Task<Users> GetOwnProfile(Guid uid)
        {
            var query = "select * from Users where UserId=@userId";
            var parameters = new DynamicParameters();
            parameters.Add("userId", uid);
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<Users>(query,parameters);
                connection.Close();
                return result;
            }
        }


        //for get all districts
        public async Task<APIResponse<IEnumerable<Districts>>> GetAllDistricts()
        {
            var query = "select * from Districts";
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<Districts>(query);
                connection.Close();
                if(result.Count()==0)
                {
                    return APIResponse<IEnumerable<Districts>>.Error("No districts in the list");
                }
                return APIResponse<IEnumerable<Districts>>.Success(result,"No districts in the list");
            }
        }


        //for update the username of the user
        public async Task<APIResponse<bool>> UpdateProfileImage(UpdateProfileImageDto proImage, Guid id)
        {
            var gettingQuery = "select * from Users where UserId=@id";
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var resut = await connection.QueryFirstOrDefaultAsync<Users>(gettingQuery, new { id = id });
                connection.Close();
                if (resut == null)
                {
                    return APIResponse<bool>.Error("No user in this id");
                }

                //image code sytarts here
                string filePath = string.Empty;
                string newFileName = string.Empty;
                if (proImage.image != null)
                {
                    var folderPath = Path.Combine(_env.WebRootPath, "Media", "ProfileImage");


                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }


                    var fileExtension = Path.GetExtension(proImage.image.FileName).ToLower();

                    var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif" };

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return APIResponse<bool>.Error("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");
                    }
                    newFileName = $"{resut.Email}{fileExtension}";
                    filePath = Path.Combine(folderPath, newFileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await proImage.image.CopyToAsync(stream);
                    }
                    Console.WriteLine(proImage.image.FileName);
                    Console.WriteLine(filePath);
                    Console.WriteLine(newFileName);
                }
                //image code ends here
                var updateQuery = "update Users set ProfileImage=@image where UserId=@userid";
                var parameters = new DynamicParameters();
                parameters.Add("userid", id);
                parameters.Add("image", newFileName);
                connection.Open();
                var rowAffected = await connection.ExecuteAsync(updateQuery, parameters);
                connection.Close();
                Console.WriteLine(rowAffected);
                if(rowAffected == 0)
                {
                    return APIResponse<bool>.Error("Unable to update the Image");
                }
                return APIResponse<bool>.Success(true,"ProfileImage Updated Successfully");

            }
        }




    }
}
