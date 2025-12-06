using StoreAPI.DTOs;
using StoreAPI.Helpers;

namespace StoreAPI.Services.Interfaces
{
    public interface IAddressService
    {
        Task<ApiResponse<List<AddressDTO>>> GetUserAddressesAsync(int userId);
        Task<ApiResponse<AddressDTO>> GetByIdAsync(int userId, int addressId);
        Task<ApiResponse<AddressDTO>> CreateAsync(int userId, CreateAddressDTO dto);
        Task<ApiResponse<AddressDTO>> UpdateAsync(int userId, int addressId, UpdateAddressDTO dto);
        Task<ApiResponse<bool>> DeleteAsync(int userId, int addressId);
    }
}