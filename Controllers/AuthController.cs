using Dynamic_Eye.DTOs;
using Dynamic_Eye.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dynamic_Eye.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAuthDto request)
        {
            var token = await _authService.AuthenticateAsync(request.email, request.password);

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalid email or password.");

            return Ok(new { Token = token });
        }
    }
}
