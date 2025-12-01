using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreAPI.Data;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Services.Implementations
{
    public class AdminAuthService : IAdminAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AdminAuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<ApiResponse<AdminUserDTO>> LoginAsync(AdminLoginDTO dto)
        {
            var admin = await _context.AdminUsers
                .FirstOrDefaultAsync(a => a.Email == dto.Email);

            if (admin == null)
                return ApiResponse<AdminUserDTO>.ErrorResponse("حساب الإدارة غير موجود.");

            if (!VerifyPassword(dto.Password, admin.PasswordHash!))
                return ApiResponse<AdminUserDTO>.ErrorResponse("كلمة المرور غير صحيحة.");

            if (admin.IsActive == false)
                return ApiResponse<AdminUserDTO>.ErrorResponse("حساب الإدارة غير مفعل.");

            // قراءة إعدادات JWT
            var jwt = _config.GetSection("Jwt");

            // توليد توكن موحد للمستخدمين والأدمن
            string token = JwtHelper.GenerateToken(
                id: admin.AdminId,
                fullName: admin.FullName,
                email: admin.Email ?? "",
                role: "Admin",
                key: jwt["Key"]!,
                issuer: jwt["Issuer"]!,
                audience: jwt["Audience"]!,
                expireMinutes: int.Parse(jwt["ExpireMinutes"]!)
            );

            var result = new AdminUserDTO
            {
                AdminId = admin.AdminId,
                FullName = admin.FullName,
                Email = admin.Email,
                Token = token
            };

            return ApiResponse<AdminUserDTO>.SuccessResponse(result, "تم تسجيل دخول الأدمن بنجاح.");
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            string hashed = Convert.ToBase64String(bytes);

            return hashed == storedHash;
        }
    }
}