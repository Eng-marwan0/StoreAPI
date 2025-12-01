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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Helper: Get UserId from Token safely
        private int GetUserId()
        {
            var idClaim = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(idClaim))
                throw new Exception("Token لا يحتوي على معرف مستخدم");

            if (!int.TryParse(idClaim, out int userId))
                throw new Exception("معرف المستخدم في التوكن غير صالح");

            return userId;
        }

        // ============================
        // 0) Create Order from Cart
        // ============================
        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderDTO>>> CreateOrder([FromBody] CreateOrderDTO dto)
        {
            int userId = GetUserId();

            var result = await _orderService.CreateOrderFromCartAsync(userId, dto);
            return Ok(result);
        }

        // ============================
        // 1) Get All Orders for User
        // ============================
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<OrderDTO>>>> GetMyOrders()
        {
            int userId = GetUserId();

            var result = await _orderService.GetUserOrdersAsync(userId);
            return Ok(result);
        }

        // ============================
        // 2) Get Order By Id
        // ============================
        [HttpGet("{orderId}")]
        public async Task<ActionResult<ApiResponse<OrderDTO>>> GetOrderById(int orderId)
        {
            int userId = GetUserId();

            var result = await _orderService.GetOrderByIdAsync(userId, orderId);
            return Ok(result);
        }

        // ============================
        // 3) Cancel Order
        // ============================
        [HttpPost("{orderId}/cancel")]
        public async Task<ActionResult<ApiResponse<bool>>> CancelOrder(int orderId)
        {
            int userId = GetUserId();

            var result = await _orderService.CancelOrderAsync(userId, orderId);
            return Ok(result);
        }
    }
}