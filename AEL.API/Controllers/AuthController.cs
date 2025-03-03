using Microsoft.AspNetCore.Mvc;
using AEL.Application.Interfaces.Services;
using AEL.Application.DTOs;

namespace Hotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly bool _allowManageUserWithoutAuth;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
            _allowManageUserWithoutAuth = configuration.GetValue<bool>("Security:AllowManageUserWithoutAuth");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var response = await _authService.LoginAsync(request);

            if (!response.Success)
            {
                return response.Message switch
                {
                    "User not found." => NotFound(response),
                    "Invalid credentials." => Unauthorized(response),
                    _ => BadRequest(response)
                };
            }

            return Ok(response);
        }
    }

}
