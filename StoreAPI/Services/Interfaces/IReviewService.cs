using StoreAPI.DTOs;
using StoreAPI.Helpers;

namespace StoreAPI.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ApiResponse<ReviewDTO>> AddReviewAsync(int userId, CreateReviewDTO dto);
        Task<ApiResponse<ReviewDTO>> UpdateReviewAsync(int userId, int reviewId, UpdateReviewDTO dto);
        Task<ApiResponse<bool>> DeleteReviewAsync(int userId, int reviewId);
        Task<ApiResponse<List<ReviewDTO>>> GetProductReviewsAsync(int productId);
    }
}