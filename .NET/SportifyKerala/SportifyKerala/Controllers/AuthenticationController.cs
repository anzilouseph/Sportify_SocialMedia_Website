using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportifyKerala.Dto;
using SportifyKerala.IService;

namespace SportifyKerala.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationManagementService _service;

        public AuthenticationController(IAuthenticationManagementService service)
        {
            _service = service;
        }

        //for login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            try
            {
                var apiResposne = await _service.Login(login);
                return Ok(apiResposne);
            }
            catch (Exception ex)
            {
                return StatusCode(500,new {message = ex.Message});
            }
        }

    }
}
