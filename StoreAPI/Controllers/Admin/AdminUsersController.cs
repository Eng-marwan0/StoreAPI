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
    [Route("api/admin/users")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminUsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminUsersController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1) جلب جميع المستخدمين مع الفلترة
        // ==========================================
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<UserDTO>>>> GetAllUsers(
            string? search, bool? isActive)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.FullName.Contains(search) ||
                    u.Email.Contains(search) ||
                    u.Phone.Contains(search));
            }

            if (isActive.HasValue)
            {
                query = query.Where(u => u.IsActive == isActive.Value);
            }

            var users = await query
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            var list = users.Select(u => new UserDTO
            {
                UserId = u.UserId,
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone,
                IsActive = u.IsActive ?? false
            }).ToList();

            return ApiResponse<List<UserDTO>>.SuccessResponse(list, "تم جلب المستخدمين بنجاح");
        }

        // ==========================================
        // 2) جلب مستخدم واحد مع التفاصيل
        // ==========================================
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDetailsDTO>>> GetUserDetails(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserAddresses)
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return ApiResponse<UserDetailsDTO>.ErrorResponse("المستخدم غير موجود");

            var dto = new UserDetailsDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive ?? false,
                CreatedAt = user.CreatedAt,
                Addresses = user.UserAddresses.Select(a => new UserAddressDTO
                {
                    AddressId = a.AddressId,
                    City = a.City,
                    Area = a.Area,
                    Street = a.Street,
                    Notes = a.Notes
                }).ToList(),
                TotalOrders = user.Orders.Count,
                LastOrderDate = user.Orders.OrderByDescending(o => o.CreatedAt).FirstOrDefault()?.CreatedAt
            };

            return ApiResponse<UserDetailsDTO>.SuccessResponse(dto, "تم جلب تفاصيل المستخدم");
        }

        // ==========================================
        // 3) تفعيل / تعطيل المستخدم
        // ==========================================
        [HttpPut("{id}/status")]
        public async Task<ActionResult<ApiResponse<bool>>> ChangeStatus(int id, [FromBody] bool active)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return ApiResponse<bool>.ErrorResponse("المستخدم غير موجود");

            user.IsActive = active;
            await _context.SaveChangesAsync();

            string msg = active ? "تم تفعيل المستخدم" : "تم تعطيل المستخدم";
            return ApiResponse<bool>.SuccessResponse(true, msg);
        }

        // ==========================================
        // 4) تحديث بيانات المستخدم
        // ==========================================
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UserDTO>>> UpdateUser(int id, UpdateUserDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return ApiResponse<UserDTO>.ErrorResponse("المستخدم غير موجود");

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;

            await _context.SaveChangesAsync();

            var result = new UserDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive ?? false
            };

            return ApiResponse<UserDTO>.SuccessResponse(result, "تم تحديث بيانات المستخدم");
        }

        // ==========================================
        // 5) حذف المستخدم بالكامل
        // ==========================================
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return ApiResponse<bool>.ErrorResponse("المستخدم غير موجود");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم حذف المستخدم بنجاح");
        }
    }
}