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

        // Get all addresses
        public async Task<ApiResponse<List<AddressDTO>>> GetUserAddressesAsync(int userId)
        {
            var addresses = await _context.UserAddresses
                .Where(a => a.UserId == userId)
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

            return ApiResponse<List<AddressDTO>>.SuccessResponse(addresses);
        }

        // Get specific address
        public async Task<ApiResponse<AddressDTO>> GetByIdAsync(int userId, int addressId)
        {
            var address = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);

            if (address == null)
                return ApiResponse<AddressDTO>.ErrorResponse("العنوان غير موجود.");

            var dto = new AddressDTO
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

            return ApiResponse<AddressDTO>.SuccessResponse(dto);
        }

        // Create address
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

            return ApiResponse<AddressDTO>.SuccessResponse(new AddressDTO
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
            }, "تم إضافة العنوان بنجاح.");
        }

        // Update
        public async Task<ApiResponse<AddressDTO>> UpdateAsync(int userId, int addressId, UpdateAddressDTO dto)
        {
            var address = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);

            if (address == null)
                return ApiResponse<AddressDTO>.ErrorResponse("العنوان غير موجود.");

            address.FullName = dto.FullName ?? address.FullName;
            address.Phone = dto.Phone ?? address.Phone;
            address.City = dto.City ?? address.City;
            address.Area = dto.Area ?? address.Area;
            address.Region = dto.Region ?? address.Region;
            address.Street = dto.Street ?? address.Street;
            address.Building = dto.Building ?? address.Building;
            address.Notes = dto.Notes ?? address.Notes;
            address.Latitude = dto.Latitude ?? address.Latitude;
            address.Longitude = dto.Longitude ?? address.Longitude;

            await _context.SaveChangesAsync();

            return ApiResponse<AddressDTO>.SuccessResponse(new AddressDTO
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
            }, "تم تعديل العنوان بنجاح.");
        }

        // Delete
        public async Task<ApiResponse<bool>> DeleteAsync(int userId, int addressId)
        {
            var address = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);

            if (address == null)
                return ApiResponse<bool>.ErrorResponse("العنوان غير موجود.");

            _context.UserAddresses.Remove(address);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم حذف العنوان.");
        }
    }
}