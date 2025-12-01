namespace StoreAPI.Services
{
    public interface IWhatsAppService
    {
        Task<bool> SendOtpAsync(string phone, string code);
    }
}