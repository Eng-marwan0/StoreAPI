using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Services;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;

        public ShippingController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }

        // ======================================================
        // Helper: Safely extract user id from JWT
        // ======================================================
        private int GetUserId()
        {
            var idClaim = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(idClaim))
                throw new Exception("التوكن لا يحتوي على userId.");

            if (!int.TryParse(idClaim, out int userId))
                throw new Exception("قيمة userId في التوكن غير صالحة.");

            return userId;
        }

        // ======================================================
        // Get all shipments for a specific order
        // ======================================================
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<ApiResponse<List<ShipmentDTO>>>> GetOrderShipments(int orderId)
        {
            int userId = GetUserId();

            var result = await _shippingService.GetOrderShipmentsAsync(userId, orderId);
            return Ok(result);
        }
    }
}