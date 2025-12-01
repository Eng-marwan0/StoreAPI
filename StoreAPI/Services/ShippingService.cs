using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;

namespace StoreAPI.Services
{
    public class ShippingService : IShippingService
    {
        private readonly AppDbContext _context;

        public ShippingService(AppDbContext context)
        {
            _context = context;
        }

        // جلب شحنات طلب معيّن لمستخدم معيّن
        public async Task<ApiResponse<List<ShipmentDTO>>> GetOrderShipmentsAsync(int userId, int orderId)
        {
            // أولاً: نتأكد أن الطلب فعلاً يخص هذا المستخدم
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
                return ApiResponse<List<ShipmentDTO>>.ErrorResponse("الطلب غير موجود أو لا يخص هذا المستخدم.");

            // ثانياً: نجلب الشحنات المرتبطة بالطلب
            var shipments = await _context.Shipments
                .Include(s => s.ShippingCompany)       // فيها اسم الشركة
                .Where(s => s.OrderId == orderId)
                .OrderBy(s => s.CreatedAt)
                .ToListAsync();

            var list = shipments.Select(s => new ShipmentDTO
            {
                ShipmentId = s.ShipmentId,
                OrderId = s.OrderId,
                ShippingCompanyId = s.ShippingCompanyId,
                // عدّل هذا السطر حسب أسماء الحقول في ShippingCompany عندك
                ShippingCompanyName = order.Shipments.FirstOrDefault()?.ShippingCompany?.Name,
                Status = s.Status ?? "",
                TrackingNumber = s.TrackingNumber,
                CreatedAt = s.CreatedAt
            }).ToList();

            return ApiResponse<List<ShipmentDTO>>.SuccessResponse(list, "تم جلب بيانات الشحن بنجاح.");
        }
    }
}