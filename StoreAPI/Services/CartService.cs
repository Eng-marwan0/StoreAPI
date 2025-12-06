using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        // ============================================================
        // جلب السلة
        // ============================================================
        public async Task<ApiResponse<CartDTO>> GetCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return ApiResponse<CartDTO>.SuccessResponse(new CartDTO());

            var dto = new CartDTO
            {
                CartId = cart.CartId,
                Items = cart.CartItems.Select(i => new CartItemDTO
                {
                    CartItemId = i.CartItemId,
                    ProductId = i.ProductId ?? 0,
                    ProductNameAr = i.Product.NameAr,
                    ProductNameEn = i.Product.NameEn,
                    ImageUrl = i.Product.MainImageUrl,
                    UnitPrice = i.UnitPrice ?? i.Product.Price ?? 0,
                    Quantity = i.Quantity ?? 1
                }).ToList()
            };

            return ApiResponse<CartDTO>.SuccessResponse(dto);
        }

        // ============================================================
        // إضافة منتج للسلة
        // ============================================================
        public async Task<ApiResponse<CartDTO>> AddItemAsync(int userId, CartAddItemDTO dto)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // إنشاء سلة إذا غير موجودة
            if (cart == null)
            {
                cart = new Cart { UserId = userId, CreatedAt = DateTime.UtcNow };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                return ApiResponse<CartDTO>.ErrorResponse("المنتج غير موجود.");

            var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == dto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = product.Price,
                    PriceAtTime = product.Price
                });
            }

            await _context.SaveChangesAsync();

            return await GetCartAsync(userId);
        }

        // ============================================================
        // تعديل عنصر
        // ============================================================
        public async Task<ApiResponse<CartDTO>> UpdateItemAsync(int userId, CartUpdateItemDTO dto)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return ApiResponse<CartDTO>.ErrorResponse("السلة غير موجودة.");

            var item = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == dto.CartItemId);

            if (item == null)
                return ApiResponse<CartDTO>.ErrorResponse("العنصر غير موجود.");

            if (dto.Quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }
            else
            {
                item.Quantity = dto.Quantity;
            }

            await _context.SaveChangesAsync();

            return await GetCartAsync(userId);
        }

        // ============================================================
        // حذف عنصر
        // ============================================================
        public async Task<ApiResponse<CartDTO>> RemoveItemAsync(int userId, int cartItemId)
        {
            var item = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId && ci.Cart.UserId == userId);

            if (item == null)
                return ApiResponse<CartDTO>.ErrorResponse("العنصر غير موجود.");

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return await GetCartAsync(userId);
        }

        // ============================================================
        // مسح السلة
        // ============================================================
        public async Task<ApiResponse<bool>> ClearCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return ApiResponse<bool>.SuccessResponse(true);

            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }
    }
}