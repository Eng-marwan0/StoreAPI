using StoreAPI.DTOs;
using StoreAPI.Helpers;

namespace StoreAPI.Services.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponse<CartDTO>> GetCartAsync(int userId);

        Task<ApiResponse<CartDTO>> AddItemAsync(int userId, CartAddItemDTO dto);

        Task<ApiResponse<CartDTO>> UpdateItemAsync(int userId, CartUpdateItemDTO dto);

        Task<ApiResponse<CartDTO>> RemoveItemAsync(int userId, int cartItemId);

        Task<ApiResponse<bool>> ClearCartAsync(int userId);
    }
}