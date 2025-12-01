namespace StoreAPI.Services.Interfaces
{
    public interface IOTPService
    {
        Task<string> GenerateOtpAsync(string phone);
        Task<bool> ValidateOtpAsync(string phone, string code);
    }
}