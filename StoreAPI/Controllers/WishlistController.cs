using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]   // لازم يكون مستخدم مسجّل دخول
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        // Helper لاستخراج UserId من التوكن
        private int GetUserId()
        {
            var idClaim = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(idClaim))
                throw new Exception("Token لا يحتوي على معرف مستخدم");

            if (!int.TryParse(idClaim, out int userId))
                throw new Exception("معرف المستخدم في التوكن غير صالح");

            return userId;
        }

        // GET api/wishlist
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<WishlistItemDTO>>>> GetMyWishlist()
        {
            int userId = GetUserId();
            var result = await _wishlistService.GetUserWishlistAsync(userId);
            return Ok(result);
        }

        // POST api/wishlist/{productId}
        [HttpPost("{productId}")]
        public async Task<ActionResult<ApiResponse<bool>>> AddToWishlist(int productId)
        {
            int userId = GetUserId();
            var result = await _wishlistService.AddAsync(userId, productId);
            return Ok(result);
        }

        // DELETE api/wishlist/{productId}
        [HttpDelete("{productId}")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveFromWishlist(int productId)
        {
            int userId = GetUserId();
            var result = await _wishlistService.RemoveAsync(userId, productId);
            return Ok(result);
        }

        // DELETE api/wishlist
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<bool>>> ClearWishlist()
        {
            int userId = GetUserId();
            var result = await _wishlistService.ClearAsync(userId);
            return Ok(result);
        }
    }
}