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

        // =============================
        // Helper: Get or create cart
        // =============================
        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        // =============================
        // Helper: Build CartDTO
        // =============================
        private async Task<CartDTO> BuildCartDtoAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                return new CartDTO
                {
                    CartId = 0,
                    Items = new List<CartItemDTO>(),
                    TotalItems = 0,
                    SubTotal = 0m
                };
            }

            var items = cart.CartItems.Select(ci =>
            {
                var quantity = ci.Quantity ?? 0;
                var unitPrice = ci.UnitPrice ?? ci.PriceAtTime ?? ci.Product?.Price ?? 0m;

                return new CartItemDTO
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId ?? 0,
                    NameAr = ci.Product?.NameAr,
                    NameEn = ci.Product?.NameEn,
                    MainImageUrl = ci.Product?.MainImageUrl,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = quantity * unitPrice
                };
            }).ToList();

            return new CartDTO
            {
                CartId = cart.CartId,
                Items = items,
                TotalItems = items.Sum(i => i.Quantity),
                SubTotal = items.Sum(i => i.TotalPrice)
            };
        }

        // =============================
        // Get Cart
        // =============================
        public async Task<ApiResponse<CartDTO>> GetCartAsync(int userId)
        {
            var dto = await BuildCartDtoAsync(userId);
            return ApiResponse<CartDTO>.SuccessResponse(dto, "تم جلب السلة بنجاح.");
        }

        // =============================
        // Add Item
        // =============================
        public async Task<ApiResponse<CartDTO>> AddItemAsync(int userId, AddToCartDTO dto)
        {
            if (dto.Quantity <= 0)
                dto.Quantity = 1;

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == dto.ProductId);

            if (product == null)
                return ApiResponse<CartDTO>.ErrorResponse("المنتج غير موجود.");

            var cart = await GetOrCreateCartAsync(userId);

            var existingItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == dto.ProductId);

            var unitPrice = product.Price ?? 0m;

            if (existingItem == null)
            {
                var newItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = product.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = unitPrice,
                    PriceAtTime = unitPrice
                };

                _context.CartItems.Add(newItem);
            }
            else
            {
                existingItem.Quantity = (existingItem.Quantity ?? 0) + dto.Quantity;
                existingItem.UnitPrice = unitPrice;
                existingItem.PriceAtTime = unitPrice;
            }

            await _context.SaveChangesAsync();

            var dtoCart = await BuildCartDtoAsync(userId);
            return ApiResponse<CartDTO>.SuccessResponse(dtoCart, "تم تحديث السلة بنجاح.");
        }

        // =============================
        // Update Item quantity
        // =============================
        public async Task<ApiResponse<CartDTO>> UpdateItemAsync(int userId, int cartItemId, UpdateCartItemDTO dto)
        {
            var cart = await GetOrCreateCartAsync(userId);

            var item = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);

            if (item == null)
                return ApiResponse<CartDTO>.ErrorResponse("العنصر غير موجود في السلة.");

            if (dto.Quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }
            else
            {
                item.Quantity = dto.Quantity;
            }

            await _context.SaveChangesAsync();

            var dtoCart = await BuildCartDtoAsync(userId);
            return ApiResponse<CartDTO>.SuccessResponse(dtoCart, "تم تحديث السلة بنجاح.");
        }

        // =============================
        // Remove Item
        // =============================
        public async Task<ApiResponse<bool>> RemoveItemAsync(int userId, int cartItemId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return ApiResponse<bool>.ErrorResponse("السلة غير موجودة.");

            var item = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);
            if (item == null)
                return ApiResponse<bool>.ErrorResponse("العنصر غير موجود في السلة.");

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم حذف العنصر من السلة.");
        }

        // =============================
        // Clear Cart
        // =============================
        public async Task<ApiResponse<bool>> ClearCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                return ApiResponse<bool>.SuccessResponse(true, "السلة فارغة بالفعل.");

            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم تفريغ السلة.");
        }
    }
}