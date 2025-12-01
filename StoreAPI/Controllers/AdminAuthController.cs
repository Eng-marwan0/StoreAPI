using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/auth")]
    public class AdminAuthController : ControllerBase
    {
        private readonly IAdminAuthService _adminAuth;

        public AdminAuthController(IAdminAuthService adminAuth)
        {
            _adminAuth = adminAuth;
        }

        // ===========================
        // Admin Login Only
        // ===========================
        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginDTO dto)
        {
            var result = await _adminAuth.LoginAsync(dto);
            return Ok(result);
        }
    }
}