using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;

namespace StoreAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/orders")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminOrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminOrdersController(AppDbContext context)
        {
            _context = context;
        }

        // ================================
        // 1) جلب جميع الطلبات
        // ================================
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<OrderDTO>>>> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(i => i.Product)
                .Include(o => o.Coupon)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var list = orders.Select(o => new OrderDTO
            {
                OrderId = o.OrderId,
                TotalAmount = o.TotalAmount,
                PaymentMethod = o.PaymentMethod,
                CreatedAt = o.CreatedAt,
                DeliveryStatus = o.Status,
                CouponCode = o.Coupon?.Code,
                DiscountAmount = 0, // يمكنك حسابه عند الحفظ
                Items = o.OrderItems.Select(i => new OrderItemDTO
                {
                    OrderItemId = i.OrderItemId,
                    ProductId = i.ProductId,
                    ProductNameAr = i.Product?.NameAr,
                    ProductNameEn = i.Product?.NameEn,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            }).ToList();

            return Ok(ApiResponse<List<OrderDTO>>.SuccessResponse(list, "تم جلب جميع الطلبات"));
        }

        // ================================
        // 2) جلب طلب معيّن بالتاريخ الكامل
        // ================================
        [HttpGet("{orderId}")]
        public async Task<ActionResult<ApiResponse<OrderDTO>>> GetOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(i => i.Product)
                .Include(o => o.Coupon)
                .Include(o => o.Shipments).ThenInclude(s => s.ShippingCompany)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return ApiResponse<OrderDTO>.ErrorResponse("الطلب غير موجود");

            var dto = new OrderDTO
            {
                OrderId = order.OrderId,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod,
                CreatedAt = order.CreatedAt,
                DeliveryStatus = order.Status,
                CouponCode = order.Coupon?.Code,
                Items = order.OrderItems.Select(i => new OrderItemDTO
                {
                    OrderItemId = i.OrderItemId,
                    ProductId = i.ProductId,
                    ProductNameAr = i.Product?.NameAr,
                    ProductNameEn = i.Product?.NameEn,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return Ok(ApiResponse<OrderDTO>.SuccessResponse(dto, "تم جلب تفاصيل الطلب"));
        }

        // ================================
        // 3) تغيير حالة الطلب
        // ================================
        [HttpPut("{orderId}/status")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
                return ApiResponse<bool>.ErrorResponse("الطلب غير موجود");

            order.Status = newStatus;
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, "تم تحديث حالة الطلب"));
        }

        // ================================
        // 4) ربط الطلب بشركة شحن
        // ================================
        [HttpPost("{orderId}/shipping")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignShippingCompany(int orderId, [FromBody] int companyId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
                return ApiResponse<bool>.ErrorResponse("الطلب غير موجود");

            var company = await _context.ShippingCompanies.FirstOrDefaultAsync(x => x.ShippingCompanyId == companyId);
            if (company == null)
                return ApiResponse<bool>.ErrorResponse("شركة الشحن غير موجودة");

            var shipment = new Shipment
            {
                OrderId = orderId,
                ShippingCompanyId = companyId,
                Status = "Assigned",
                CreatedAt = DateTime.UtcNow
            };

            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, "تم ربط شركة الشحن بالطلب"));
        }

        // ================================
        // 5) إلغاء الطلب بالكامل
        // ================================
        [HttpPut("{orderId}/cancel")]
        public async Task<ActionResult<ApiResponse<bool>>> CancelOrder(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
                return ApiResponse<bool>.ErrorResponse("الطلب غير موجود");

            order.Status = "Canceled";
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, "تم إلغاء الطلب بنجاح"));
        }
    }
}