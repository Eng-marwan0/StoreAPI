using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.Helpers;

namespace StoreAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminDashboardController(AppDbContext context)
        {
            _context = context;
        }

        // ==============================
        // GET: ملخص عام للوحة التحكم
        // api/admin/dashboard/summary
        // ==============================
        [HttpGet("summary")]
        public async Task<ActionResult<ApiResponse<object>>> GetSummary()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalOrders = await _context.Orders.CountAsync();
            var pendingOrders = await _context.Orders
                .CountAsync(o => o.Status == "Pending" || o.Status == "New");
            var totalSales = await _context.Orders
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0m;

            var data = new
            {
                TotalUsers = totalUsers,
                TotalOrders = totalOrders,
                PendingOrders = pendingOrders,
                TotalSales = totalSales
            };

            return Ok(ApiResponse<object>.SuccessResponse(data, "تم جلب إحصائيات لوحة التحكم بنجاح."));
        }
    }
}