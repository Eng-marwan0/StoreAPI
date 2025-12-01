using StoreAPI.DTOs;
using StoreAPI.Helpers;

namespace StoreAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ApiResponse<OrderDTO>> CreateOrderFromCartAsync(int userId, CreateOrderDTO dto);

        Task<ApiResponse<OrderDTO>> GetOrderByIdAsync(int userId, int orderId);

        Task<ApiResponse<List<OrderDTO>>> GetUserOrdersAsync(int userId);

        Task<ApiResponse<bool>> CancelOrderAsync(int userId, int orderId);
    }
}