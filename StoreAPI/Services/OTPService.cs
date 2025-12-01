using StoreAPI.Data;
using StoreAPI.Models;
using StoreAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Services.Implementations
{
    public class OTPService : IOTPService
    {
        private readonly AppDbContext _context;

        public OTPService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateOtpAsync(string phone)
        {
            string code = new Random().Next(100000, 999999).ToString();

            var otp = new OTP
            {
                Phone = phone,
                Code = code,
                Purpose = "Register",
                Attempts = 0,
                IsUsed = false,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

            _context.OTPs.Add(otp);
            await _context.SaveChangesAsync();

            return code;
        }

        public async Task<bool> ValidateOtpAsync(string phone, string code)
        {
            var otp = await _context.OTPs
                .Where(o => o.Phone == phone)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (otp == null)
                return false;

            if (otp.IsUsed)
                return false;

            if (DateTime.UtcNow > otp.ExpiresAt)
                return false;

            return otp.Code == code;
        }
    }
}