using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Services;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // =============================
        // Register
        // =============================
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }

        // =============================
        // Login
        // =============================
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }
    }
}