using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shipping;

        public ShippingController(IShippingService shipping)
        {
            _shipping = shipping;
        }

        // جلب شحنات الطلب للمستخدم
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderShipments(int orderId)
        {
            var id = User.FindFirst("id")?.Value;

            if (!int.TryParse(id, out int userId))
                return Unauthorized("Invalid token.");

            var result = await _shipping.GetOrderShipmentsAsync(userId, orderId);

            return Ok(result);
        }
    }
}