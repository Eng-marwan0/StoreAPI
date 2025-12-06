using StoreAPI.DTOs;
using StoreAPI.Helpers;

namespace StoreAPI.Services.Interfaces
{
    public interface IShippingCompanyService
    {
        Task<ApiResponse<List<ShippingCompanyDTO>>> GetAllAsync();
        Task<ApiResponse<ShippingCompanyDTO>> GetByIdAsync(int id);
        Task<ApiResponse<ShippingCompanyDTO>> CreateAsync(CreateShippingCompanyDTO dto);
        Task<ApiResponse<ShippingCompanyDTO>> UpdateAsync(int id, UpdateShippingCompanyDTO dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}