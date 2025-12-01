using StoreAPI.DTOs;
using StoreAPI.Helpers;

namespace StoreAPI.Services.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponse<CartDTO>> GetCartAsync(int userId);
        Task<ApiResponse<CartDTO>> AddItemAsync(int userId, AddToCartDTO dto);
        Task<ApiResponse<CartDTO>> UpdateItemAsync(int userId, int cartItemId, UpdateCartItemDTO dto);
        Task<ApiResponse<bool>> RemoveItemAsync(int userId, int cartItemId);
        Task<ApiResponse<bool>> ClearCartAsync(int userId);
    }
}