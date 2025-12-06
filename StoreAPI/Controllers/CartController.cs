using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private int GetUserId()
        {
            var claim = User.FindFirst("id")?.Value;
            return int.Parse(claim);
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            int userId = GetUserId();
            var result = await _cartService.GetCartAsync(userId);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(CartAddItemDTO dto)
        {
            int userId = GetUserId();
            var result = await _cartService.AddItemAsync(userId, dto);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(CartUpdateItemDTO dto)
        {
            int userId = GetUserId();
            var result = await _cartService.UpdateItemAsync(userId, dto);
            return Ok(result);
        }

        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            int userId = GetUserId();
            var result = await _cartService.RemoveItemAsync(userId, cartItemId);
            return Ok(result);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            int userId = GetUserId();
            var result = await _cartService.ClearCartAsync(userId);
            return Ok(result);
        }
    }
}