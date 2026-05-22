using KMC_API.DTOs;
using KMC_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace KMC_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.Register(request);
            if (result == null)
                return BadRequest(new { message = "Email already in use." });
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.Login(request);
            if (result == null)
                return Unauthorized(new { message = "Invalid email or password." });
            return Ok(result);
        }
    }
}