using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportifyKerala.Dto;
using SportifyKerala.IRepo;

namespace SportifyKerala.Controllers
{
    [Route("api/FollowManagement")]
    [ApiController]
    public class FollowManagementController : ControllerBase
    {
        private readonly IFollowManagement _repo;
      

        public FollowManagementController(IFollowManagement repo)
        {
            _repo = repo;
            
        }


        //to follow a club (he needs to accept it)
        [Authorize]
        [HttpPost("Follow")]
        public async Task<IActionResult> Follow(FollowDto followData)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userid))
                {
                    return Unauthorized("JWT GENERATION ERROR");
                }
                var apiResponse = await _repo.Follow(followData, userid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500,new { message = ex.Message });

            }
        }


        //for get all the requests
        [Authorize]
        [HttpGet("FollowRequests")]
        public async Task<IActionResult> GetRequests()
        {
            try
            {
                var clubIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (clubIdClaim == null || !Guid.TryParse(clubIdClaim.Value, out Guid clubid))
                {
                    return Unauthorized("JWT GENERATION ERROR");
                }
                var apiResponse = await _repo.GetRequests(clubid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }



        //for  accept the follow of a particular user by the Company
        [Authorize]
        [HttpPut("AcceptRequest")]
        public async Task<IActionResult> AcceptRequest(Guid userid)
        {
            try
            {
                var clubIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (clubIdClaim == null || !Guid.TryParse(clubIdClaim.Value, out Guid clubid))
                {
                    return Unauthorized("JWT GENERATION ERROR");
                }
                var apiResponse = await _repo.AcceptRequest(userid, clubid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });

            }
        }

        //for  Reject the follow of a particular user by the Company
        [Authorize]
        [HttpDelete("RejectRequest")]
        public async Task<IActionResult> RejectRequest(Guid userid)
        {
            try
            {
                var clubIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (clubIdClaim == null || !Guid.TryParse(clubIdClaim.Value, out Guid clubid))
                {
                    return Unauthorized("JWT GENERATION ERROR");
                }
                var apiResponse = await _repo.RejectRequest(userid,clubid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
