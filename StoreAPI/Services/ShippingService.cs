using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Services
{
    public class ShippingService : IShippingService
    {
        private readonly AppDbContext _context;

        public ShippingService(AppDbContext context)
        {
            _context = context;
        }

        // =============================
        // (User) جلب شحنات طلب معيّن للمستخدم
        // =============================
        public async Task<ApiResponse<List<ShipmentDTO>>> GetOrderShipmentsAsync(int userId, int orderId)
        {
            // تأكد أن الطلب للمستخدم هذا
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
                return ApiResponse<List<ShipmentDTO>>.ErrorResponse("الطلب غير موجود أو لا يخص هذا المستخدم.");

            var shipments = await _context.Shipments
                .Include(s => s.ShippingCompany)
                .Where(s => s.OrderId == orderId)
                .OrderBy(s => s.CreatedAt)
                .ToListAsync();

            var list = shipments.Select(s => new ShipmentDTO
            {
                ShipmentId = s.ShipmentId,
                OrderId = s.OrderId,
                ShippingCompanyId = s.ShippingCompanyId,
                ShippingCompanyName = s.ShippingCompany?.Name,
                Status = s.Status ?? "",
                TrackingNumber = s.TrackingNumber,
                CreatedAt = s.CreatedAt,
                DeliveredAt = null // إن كان عندك حقل في DB أضفه هنا
            }).ToList();

            return ApiResponse<List<ShipmentDTO>>.SuccessResponse(list, "تم جلب بيانات الشحن بنجاح.");
        }

        // =============================
        // (Admin) جلب كل الشحنات
        // =============================
        public async Task<ApiResponse<List<ShipmentDTO>>> GetAllShipmentsAsync()
        {
            var shipments = await _context.Shipments
                .Include(s => s.ShippingCompany)
                .Include(s => s.Order)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            var list = shipments.Select(s => new ShipmentDTO
            {
                ShipmentId = s.ShipmentId,
                OrderId = s.OrderId,
                ShippingCompanyId = s.ShippingCompanyId,
                ShippingCompanyName = s.ShippingCompany?.Name,
                Status = s.Status ?? "",
                TrackingNumber = s.TrackingNumber,
                CreatedAt = s.CreatedAt,
                DeliveredAt = null
            }).ToList();

            return ApiResponse<List<ShipmentDTO>>.SuccessResponse(list);
        }

        // =============================
        // (Admin) جلب شحنة واحدة
        // =============================
        public async Task<ApiResponse<ShipmentDTO>> GetByIdAsync(int shipmentId)
        {
            var s = await _context.Shipments
                .Include(x => x.ShippingCompany)
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x => x.ShipmentId == shipmentId);

            if (s == null)
                return ApiResponse<ShipmentDTO>.ErrorResponse("الشحنة غير موجودة.");

            var dto = new ShipmentDTO
            {
                ShipmentId = s.ShipmentId,
                OrderId = s.OrderId,
                ShippingCompanyId = s.ShippingCompanyId,
                ShippingCompanyName = s.ShippingCompany?.Name,
                Status = s.Status ?? "",
                TrackingNumber = s.TrackingNumber,
                CreatedAt = s.CreatedAt,
                DeliveredAt = null
            };

            return ApiResponse<ShipmentDTO>.SuccessResponse(dto);
        }

        // =============================
        // (Admin) إنشاء شحنة جديدة لطلب
        // =============================
        public async Task<ApiResponse<ShipmentDTO>> CreateShipmentAsync(CreateShipmentDTO dto)
        {
            // تأكد أن الطلب موجود
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == dto.OrderId);

            if (order == null)
                return ApiResponse<ShipmentDTO>.ErrorResponse("الطلب غير موجود.");

            // تأكد أن شركة الشحن موجودة
            var company = await _context.ShippingCompanies
                .FirstOrDefaultAsync(c => c.ShippingCompanyId == dto.ShippingCompanyId && c.IsActive);

            if (company == null)
                return ApiResponse<ShipmentDTO>.ErrorResponse("شركة الشحن غير موجودة أو غير مفعّلة.");

            var shipment = new Shipment
            {
                OrderId = dto.OrderId,
                ShippingCompanyId = dto.ShippingCompanyId,
                Status = "Pending",
                TrackingNumber = dto.TrackingNumber,
                CreatedAt = DateTime.UtcNow
            };

            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync();

            var result = new ShipmentDTO
            {
                ShipmentId = shipment.ShipmentId,
                OrderId = shipment.OrderId,
                ShippingCompanyId = shipment.ShippingCompanyId,
                ShippingCompanyName = company.Name,
                Status = shipment.Status,
                TrackingNumber = shipment.TrackingNumber,
                CreatedAt = shipment.CreatedAt
            };

            return ApiResponse<ShipmentDTO>.SuccessResponse(result, "تم إنشاء الشحنة بنجاح.");
        }

        // =============================
        // (Admin) تحديث حالة الشحنة
        // =============================
        public async Task<ApiResponse<ShipmentDTO>> UpdateShipmentStatusAsync(int shipmentId, UpdateShipmentStatusDTO dto)
        {
            var s = await _context.Shipments
                .Include(x => x.ShippingCompany)
                .FirstOrDefaultAsync(x => x.ShipmentId == shipmentId);

            if (s == null)
                return ApiResponse<ShipmentDTO>.ErrorResponse("الشحنة غير موجودة.");

            s.Status = dto.Status;
            if (!string.IsNullOrWhiteSpace(dto.TrackingNumber))
                s.TrackingNumber = dto.TrackingNumber;

            await _context.SaveChangesAsync();

            var result = new ShipmentDTO
            {
                ShipmentId = s.ShipmentId,
                OrderId = s.OrderId,
                ShippingCompanyId = s.ShippingCompanyId,
                ShippingCompanyName = s.ShippingCompany?.Name,
                Status = s.Status ?? "",
                TrackingNumber = s.TrackingNumber,
                CreatedAt = s.CreatedAt,
                DeliveredAt = null
            };

            return ApiResponse<ShipmentDTO>.SuccessResponse(result, "تم تحديث حالة الشحنة بنجاح.");
        }
    }
}