using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly AppDbContext _context;

        public AddressService(AppDbContext context)
        {
            _context = context;
        }

        // ===============================
        // Get all addresses for a user
        // ===============================
        public async Task<ApiResponse<List<AddressDTO>>> GetUserAddressesAsync(int userId)
        {
            var list = await _context.UserAddresses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AddressId)
                .Select(a => new AddressDTO
                {
                    AddressId = a.AddressId,
                    FullName = a.FullName,
                    Phone = a.Phone,
                    City = a.City,
                    Area = a.Area,
                    Region = a.Region,
                    Street = a.Street,
                    Building = a.Building,
                    Notes = a.Notes,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude
                })
                .ToListAsync();

            return ApiResponse<List<AddressDTO>>.SuccessResponse(list, "تم جلب العناوين بنجاح.");
        }

        // ===============================
        // Get single address
        // ===============================
        public async Task<ApiResponse<AddressDTO>> GetByIdAsync(int userId, int addressId)
        {
            var a = await _context.UserAddresses
                .FirstOrDefaultAsync(x => x.AddressId == addressId && x.UserId == userId);

            if (a == null)
                return ApiResponse<AddressDTO>.ErrorResponse("العنوان غير موجود.");

            var dto = new AddressDTO
            {
                AddressId = a.AddressId,
                FullName = a.FullName,
                Phone = a.Phone,
                City = a.City,
                Area = a.Area,
                Region = a.Region,
                Street = a.Street,
                Building = a.Building,
                Notes = a.Notes,
                Latitude = a.Latitude,
                Longitude = a.Longitude
            };

            return ApiResponse<AddressDTO>.SuccessResponse(dto, "تم جلب العنوان بنجاح.");
        }

        // ===============================
        // Create
        // ===============================
        public async Task<ApiResponse<AddressDTO>> CreateAsync(int userId, CreateAddressDTO dto)
        {
            var address = new UserAddress
            {
                UserId = userId,
                FullName = dto.FullName,
                Phone = dto.Phone,
                City = dto.City,
                Area = dto.Area,
                Region = dto.Region,
                Street = dto.Street,
                Building = dto.Building,
                Notes = dto.Notes,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserAddresses.Add(address);
            await _context.SaveChangesAsync();

            var result = new AddressDTO
            {
                AddressId = address.AddressId,
                FullName = address.FullName,
                Phone = address.Phone,
                City = address.City,
                Area = address.Area,
                Region = address.Region,
                Street = address.Street,
                Building = address.Building,
                Notes = address.Notes,
                Latitude = address.Latitude,
                Longitude = address.Longitude
            };

            return ApiResponse<AddressDTO>.SuccessResponse(result, "تم إنشاء العنوان بنجاح.");
        }

        // ===============================
        // Update
        // ===============================
        public async Task<ApiResponse<AddressDTO>> UpdateAsync(int userId, int addressId, UpdateAddressDTO dto)
        {
            var address = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);

            if (address == null)
                return ApiResponse<AddressDTO>.ErrorResponse("العنوان غير موجود.");

            address.FullName = dto.FullName;
            address.Phone = dto.Phone;
            address.City = dto.City;
            address.Area = dto.Area;
            address.Region = dto.Region;
            address.Street = dto.Street;
            address.Building = dto.Building;
            address.Notes = dto.Notes;
            address.Latitude = dto.Latitude;
            address.Longitude = dto.Longitude;

            await _context.SaveChangesAsync();

            var result = new AddressDTO
            {
                AddressId = address.AddressId,
                FullName = address.FullName,
                Phone = address.Phone,
                City = address.City,
                Area = address.Area,
                Region = address.Region,
                Street = address.Street,
                Building = address.Building,
                Notes = address.Notes,
                Latitude = address.Latitude,
                Longitude = address.Longitude
            };

            return ApiResponse<AddressDTO>.SuccessResponse(result, "تم تحديث العنوان بنجاح.");
        }

        // ===============================
        // Delete
        // ===============================
        public async Task<ApiResponse<bool>> DeleteAsync(int userId, int addressId)
        {
            var address = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);

            if (address == null)
                return ApiResponse<bool>.ErrorResponse("العنوان غير موجود.");

            _context.UserAddresses.Remove(address);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم حذف العنوان بنجاح.");
        }
    }
}