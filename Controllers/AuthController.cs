using Dynamic_Eye.DTOs;
using Dynamic_Eye.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dynamic_Eye.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var token = await _authService.AuthenticateAsync(request.Username!, request.Password!);

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalid username or password.");

            return Ok(new { Token = token });
        }
    }
}
