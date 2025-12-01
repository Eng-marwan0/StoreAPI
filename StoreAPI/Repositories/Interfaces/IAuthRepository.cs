using StoreAPI.Models;
using static System.Net.WebRequestMethods;

namespace StoreAPI.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> RegisterAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByPhoneAsync(string phone);
        Task<bool> UserExistsAsync(string email, string phone);
        Task SaveOTPAsync(OTP otp);
        Task<OTP?> GetLatestOTPAsync(string phone);
    }
}