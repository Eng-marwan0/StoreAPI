using StoreAPI.DTOs;
using StoreAPI.Helpers;

namespace StoreAPI.Services.Interfaces
{
    public interface IShippingService
    {
        // للـ User: جلب شحنات طلب معيّن
        Task<ApiResponse<List<ShipmentDTO>>> GetOrderShipmentsAsync(int userId, int orderId);

        // للـ Admin: جلب كل الشحنات
        Task<ApiResponse<List<ShipmentDTO>>> GetAllShipmentsAsync();

        // للـ Admin: جلب شحنة واحدة
        Task<ApiResponse<ShipmentDTO>> GetByIdAsync(int shipmentId);

        // للـ Admin: إنشاء شحنة جديدة لطلب
        Task<ApiResponse<ShipmentDTO>> CreateShipmentAsync(CreateShipmentDTO dto);

        // للـ Admin: تحديث حالة الشحنة
        Task<ApiResponse<ShipmentDTO>> UpdateShipmentStatusAsync(int shipmentId, UpdateShipmentStatusDTO dto);
    }
}