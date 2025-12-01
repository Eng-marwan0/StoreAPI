using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreAPI.Data;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // ============================
        // REGISTER
        // ============================
        public async Task<ApiResponse<UserDTO>> RegisterAsync(RegisterDTO dto)
        {
            bool exists = await _context.Users
                .AnyAsync(u => u.Email == dto.Email || u.Phone == dto.Phone);

            if (exists)
                return ApiResponse<UserDTO>.ErrorResponse("البريد الإلكتروني أو الهاتف مستخدم مسبقاً");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                PasswordHash = HashPassword(dto.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = new UserDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive ?? true,
                Token = null // لا نرجع توكن في التسجيل، فقط في تسجيل الدخول
            };

            return ApiResponse<UserDTO>.SuccessResponse(userDto, "تم إنشاء الحساب بنجاح");
        }

        // ============================
        // LOGIN (JWT موحد)
        // ============================
        public async Task<ApiResponse<UserDTO>> LoginAsync(LoginDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == dto.EmailOrPhone ||
                    u.Phone == dto.EmailOrPhone);

            if (user == null)
                return ApiResponse<UserDTO>.ErrorResponse("المستخدم غير موجود");

            if (!VerifyPassword(dto.Password, user.PasswordHash!))
                return ApiResponse<UserDTO>.ErrorResponse("كلمة المرور غير صحيحة");

            if (user.IsActive == false)
                return ApiResponse<UserDTO>.ErrorResponse("الحساب غير مفعل");

            // بيانات Jwt من appsettings.json
            var jwt = _config.GetSection("Jwt");

            string token = JwtHelper.GenerateToken(
                id: user.UserId,
                fullName: user.FullName,
                email: user.Email ?? string.Empty,
                role: "User",
                key: jwt["Key"]!,
                issuer: jwt["Issuer"]!,
                audience: jwt["Audience"]!,
                expireMinutes: int.Parse(jwt["ExpireMinutes"]!)
            );

            var dtoResult = new UserDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive ?? true,
                Token = token
            };

            return ApiResponse<UserDTO>.SuccessResponse(dtoResult, "تم تسجيل الدخول بنجاح");
        }

        // ============================
        // Helpers
        // ============================
        private string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}