using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // Add Review
        // ============================
        public async Task<ApiResponse<ReviewDTO>> AddReviewAsync(int userId, CreateReviewDTO dto)
        {
            // هل يوجد تقييم سابق لنفس المنتج ونفس المستخدم؟
            var existing = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == dto.ProductId);

            if (existing != null)
                return ApiResponse<ReviewDTO>.ErrorResponse("لقد قمت بتقييم هذا المنتج مسبقاً.");

            var review = new Review
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return ApiResponse<ReviewDTO>.SuccessResponse(new ReviewDTO
            {
                ReviewId = review.ReviewId,
                Rating = review.Rating,
                Comment = review.Comment,
                UserFullName = (await _context.Users.FindAsync(userId)).FullName,
                CreatedAt = review.CreatedAt
            }, "تم إضافة التقييم.");
        }

        // ============================
        // Update Review
        // ============================
        public async Task<ApiResponse<ReviewDTO>> UpdateReviewAsync(int userId, int reviewId, UpdateReviewDTO dto)
        {
            var review = await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.UserId == userId);

            if (review == null)
                return ApiResponse<ReviewDTO>.ErrorResponse("التقييم غير موجود.");

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;
            review.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ApiResponse<ReviewDTO>.SuccessResponse(new ReviewDTO
            {
                ReviewId = review.ReviewId,
                Rating = review.Rating,
                Comment = review.Comment,
                UserFullName = review.User.FullName,
                CreatedAt = review.CreatedAt
            }, "تم تحديث التقييم.");
        }

        // ============================
        // Delete Review
        // ============================
        public async Task<ApiResponse<bool>> DeleteReviewAsync(int userId, int reviewId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.UserId == userId);

            if (review == null)
                return ApiResponse<bool>.ErrorResponse("التقييم غير موجود.");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم حذف التقييم.");
        }

        // ============================
        // Get product reviews
        // ============================
        public async Task<ApiResponse<List<ReviewDTO>>> GetProductReviewsAsync(int productId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            var list = reviews.Select(r => new ReviewDTO
            {
                ReviewId = r.ReviewId,
                Rating = r.Rating,
                Comment = r.Comment,
                UserFullName = r.User.FullName,
                CreatedAt = r.CreatedAt
            }).ToList();

            return ApiResponse<List<ReviewDTO>>.SuccessResponse(list, "تم جلب التقييمات.");
        }
    }
}