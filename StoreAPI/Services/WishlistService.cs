using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly AppDbContext _context;

        public WishlistService(AppDbContext context)
        {
            _context = context;
        }

        // جلب مفضلة المستخدم
        public async Task<ApiResponse<List<WishlistItemDTO>>> GetUserWishlistAsync(int userId)
        {
            var items = await _context.WishlistItems
                .Where(w => w.UserId == userId)
                .Include(w => w.Product)
                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync();

            var result = items.Select(w => new WishlistItemDTO
            {
                ProductId = w.ProductId,
                NameAr = w.Product?.NameAr,
                NameEn = w.Product?.NameEn,
                Price = w.Product?.Price ?? 0,
                MainImageUrl = w.Product?.MainImageUrl,
                InStock = (w.Product?.Stock ?? 0) > 0
            }).ToList();

            return ApiResponse<List<WishlistItemDTO>>.SuccessResponse(result, "تم جلب المفضلة بنجاح.");
        }

        // إضافة منتج إلى المفضلة
        public async Task<ApiResponse<bool>> AddAsync(int userId, int productId)
        {
            // تحقق أن المنتج موجود
            var productExists = await _context.Products.AnyAsync(p => p.ProductId == productId);
            if (!productExists)
                return ApiResponse<bool>.ErrorResponse("المنتج غير موجود.");

            // منع التكرار
            var exists = await _context.WishlistItems
                .AnyAsync(w => w.UserId == userId && w.ProductId == productId);

            if (exists)
                return ApiResponse<bool>.SuccessResponse(true, "المنتج موجود مسبقاً في المفضلة.");

            var item = new WishlistItem
            {
                UserId = userId,
                ProductId = productId,
                CreatedAt = DateTime.UtcNow
            };

            _context.WishlistItems.Add(item);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم إضافة المنتج إلى المفضلة.");
        }

        // حذف منتج من المفضلة
        public async Task<ApiResponse<bool>> RemoveAsync(int userId, int productId)
        {
            var item = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (item == null)
                return ApiResponse<bool>.ErrorResponse("المنتج غير موجود في المفضلة.");

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم حذف المنتج من المفضلة.");
        }

        // مسح المفضلة بالكامل
        public async Task<ApiResponse<bool>> ClearAsync(int userId)
        {
            var items = await _context.WishlistItems
                .Where(w => w.UserId == userId)
                .ToListAsync();

            if (items.Count == 0)
                return ApiResponse<bool>.SuccessResponse(true, "المفضلة فارغة بالفعل.");

            _context.WishlistItems.RemoveRange(items);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم مسح المفضلة بالكامل.");
        }
    }
}