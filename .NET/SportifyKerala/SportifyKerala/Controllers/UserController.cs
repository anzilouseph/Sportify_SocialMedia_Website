using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SportifyKerala.Dto;
using SportifyKerala.IRepo;
using SportifyKerala.IService;
using SportifyKerala.Utilitys;

namespace SportifyKerala.Controllers
{
    [Route("api/UserManagement")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManagementService _service;
        private readonly IUserManagementRepo _repo;
        private readonly IWebHostEnvironment _env;

        public UserController(IUserManagementService service, IUserManagementRepo repo, IWebHostEnvironment env)
        {
            _service = service;
            _repo = repo;
            _env = env;
        }


        //for User Registration
        [HttpPost("UserRegistartion")]
        public async Task<IActionResult> UserRegistration(UserForCreationDto user)
        {
            try
            {
                var apiResponse = await _service.UserRegistration(user);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //for Club Registration
        [HttpPost("ClubRegistartion")]
        public async Task<IActionResult> ClubRegistration(UserForCreationDto user)
        {
            try
            {
                var apiResponse = await _service.ClubRegistration(user);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //to check the Otp is correct or not
        [HttpPost("OtpVerification")]
        public async Task<IActionResult> VerifyOtp(string otp)
        {
            try
            {
                var apiResponse = await _service.VerifyOtp(otp);

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }



        //for get own data
        [Authorize]
        [HttpGet("GetOwnData")]
        public async Task<IActionResult> GetOwnProfile()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userid))
                {
                    return Unauthorized(APIResponse<bool>.Error("Unable to generate JWT"));
                }
                var apiResponse = await _service.GetOwnProfile(userid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }



        //for all districts
        [HttpGet("GetAllDistricts")]
        public async Task<IActionResult> GetAllDistricts()
        {
            try
            {
                var apiResponse = await _repo.GetAllDistricts();
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        //foe updaet profile Image
        [HttpPut("UpdateProfileImage")]
        public async Task<IActionResult> UpdateProfileImage([FromForm]UpdateProfileImageDto proImage)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userid))
                {
                    return Unauthorized(APIResponse<bool>.Error("Unable to generate JWT"));
                }
                var apiResponse = await _repo.UpdateProfileImage(proImage,userid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //for retrive the Profile Image
        [HttpGet("getProfileImage")]
        public IActionResult GetProfileImage(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return BadRequest("Filename is not provided.");
                var filePath = Path.Combine(_env.WebRootPath, "Media/ProfileImage", fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }
                var fileExtension = Path.GetExtension(fileName).ToLower();

                string contentType = fileExtension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    _ => "application/octet-stream",
                };

                var bytes = System.IO.File.ReadAllBytes(filePath);

                return File(bytes, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
