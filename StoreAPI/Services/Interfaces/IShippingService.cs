using StoreAPI.DTOs;
using StoreAPI.Helpers;

namespace StoreAPI.Services
{
    public interface IShippingService
    {
        // جلب كل شحنات طلب معيّن لمستخدم معيّن
        Task<ApiResponse<List<ShipmentDTO>>> GetOrderShipmentsAsync(int userId, int orderId);
    }
}