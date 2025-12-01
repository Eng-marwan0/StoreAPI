using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.Models;
using StoreAPI.Repositories.Interfaces;
using static System.Net.WebRequestMethods;

namespace StoreAPI.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByPhoneAsync(string phone)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Phone == phone);
        }

        public async Task<bool> UserExistsAsync(string email, string phone)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email || u.Phone == phone);
        }

        public async Task SaveOTPAsync(OTP otp)
        {
            _context.OTPs.Add(otp);
            await _context.SaveChangesAsync();
        }

        public async Task<OTP?> GetLatestOTPAsync(string phone)
        {
            return await _context.OTPs
                .Where(o => o.Phone == phone)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}