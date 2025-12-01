using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Helper لقراءة id من الـ JWT
        private int GetUserId()
        {
            var idClaim = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(idClaim))
                throw new Exception("التوكن لا يحتوي على معرف المستخدم.");

            if (!int.TryParse(idClaim, out int userId))
                throw new Exception("معرف المستخدم في التوكن غير صالح.");

            return userId;
        }

        // GET: api/Cart
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CartDTO>>> GetCart()
        {
            int userId = GetUserId();
            var result = await _cartService.GetCartAsync(userId);
            return Ok(result);
        }

        // POST: api/Cart/items
        [HttpPost("items")]
        public async Task<ActionResult<ApiResponse<CartDTO>>> AddItem([FromBody] AddToCartDTO dto)
        {
            int userId = GetUserId();
            var result = await _cartService.AddItemAsync(userId, dto);
            return Ok(result);
        }

        // PUT: api/Cart/items/5
        [HttpPut("items/{cartItemId}")]
        public async Task<ActionResult<ApiResponse<CartDTO>>> UpdateItem(int cartItemId, [FromBody] UpdateCartItemDTO dto)
        {
            int userId = GetUserId();
            var result = await _cartService.UpdateItemAsync(userId, cartItemId, dto);
            return Ok(result);
        }

        // DELETE: api/Cart/items/5
        [HttpDelete("items/{cartItemId}")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveItem(int cartItemId)
        {
            int userId = GetUserId();
            var result = await _cartService.RemoveItemAsync(userId, cartItemId);
            return Ok(result);
        }

        // DELETE: api/Cart
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<bool>>> ClearCart()
        {
            int userId = GetUserId();
            var result = await _cartService.ClearCartAsync(userId);
            return Ok(result);
        }
    }
}