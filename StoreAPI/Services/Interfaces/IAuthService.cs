using StoreAPI.DTOs;
using StoreAPI.Helpers;

namespace StoreAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<UserDTO>> RegisterAsync(RegisterDTO dto);
        Task<ApiResponse<UserDTO>> LoginAsync(LoginDTO dto);
    }
}