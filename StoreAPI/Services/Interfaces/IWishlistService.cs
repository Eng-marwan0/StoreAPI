using StoreAPI.DTOs;
using StoreAPI.Helpers;

namespace StoreAPI.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<ApiResponse<List<WishlistItemDTO>>> GetUserWishlistAsync(int userId);
        Task<ApiResponse<bool>> AddAsync(int userId, int productId);
        Task<ApiResponse<bool>> RemoveAsync(int userId, int productId);
        Task<ApiResponse<bool>> ClearAsync(int userId);
    }
}