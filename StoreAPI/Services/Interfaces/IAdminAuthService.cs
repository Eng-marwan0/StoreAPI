using StoreAPI.DTOs;
using StoreAPI.Helpers;

namespace StoreAPI.Services.Interfaces
{
    public interface IAdminAuthService
    {
        Task<ApiResponse<AdminUserDTO>> LoginAsync(AdminLoginDTO dto);
    }
}