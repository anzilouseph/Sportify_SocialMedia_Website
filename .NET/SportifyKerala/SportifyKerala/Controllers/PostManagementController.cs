using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportifyKerala.Dto;
using SportifyKerala.IRepo;

namespace SportifyKerala.Controllers
{
    [Route("api/PostManagement")]
    [ApiController]
    public class PostManagementController : ControllerBase
    {
        private readonly IPostManagementRepo _repo;
        private readonly IWebHostEnvironment _env;

        public PostManagementController(IPostManagementRepo repo,IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }


        //for add a post
        [Authorize]
        [HttpPost("AddPost")]
        public async Task<IActionResult> AddPost(AddPostDto post)
        {
            try
            {
                var clubIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if(clubIdClaim == null || !Guid.TryParse(clubIdClaim.Value ,out Guid clubid))
                {
                    return Unauthorized("JWT GENERATION ERROR");
                }
                var apiResponse = await _repo.AddPost(post, clubid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        //for retrive the image
        [HttpGet("GetPostImage")]
        public IActionResult GetPostImage(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return BadRequest("Filename is not provided.");
                var filePath = Path.Combine(_env.WebRootPath, "Media/Posts", fileName);
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

        //for GetAllPosts

        [HttpGet("GetAllPost")]
        public async Task<IActionResult> GetAllPost()
        {
            try
            {
                var apiResponse = await _repo.GetAllPost();
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //for get all categories
        [HttpGet("GetAllCategorys")]
        public async Task<IActionResult> GetAllCategorys()
        {
            try
            {
                var apiResponse = await _repo.GetAllCategorys();
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        //for get the name and Image of the club(To see who posted the image)
        [Authorize]
        [HttpGet("GetNameandImage")]
        public async Task<IActionResult> GetNameandImage(Guid clubid) 
        {
            try
            {
               
                var apiResponse = await _repo.GetNameandImage(clubid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
