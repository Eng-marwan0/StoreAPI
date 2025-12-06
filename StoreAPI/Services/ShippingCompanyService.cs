using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Services
{
    public class ShippingCompanyService : IShippingCompanyService
    {
        private readonly AppDbContext _context;

        public ShippingCompanyService(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // 1) Get All
        // ============================
        public async Task<ApiResponse<List<ShippingCompanyDTO>>> GetAllAsync()
        {
            var list = await _context.ShippingCompanies
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ShippingCompanyDTO
                {
                    ShippingCompanyId = x.ShippingCompanyId,
                    Name = x.Name,
                    ContactNumber = x.ContactNumber,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return ApiResponse<List<ShippingCompanyDTO>>.SuccessResponse(list);
        }

        // ============================
        // 2) Get By Id
        // ============================
        public async Task<ApiResponse<ShippingCompanyDTO>> GetByIdAsync(int id)
        {
            var company = await _context.ShippingCompanies.FindAsync(id);

            if (company == null)
                return ApiResponse<ShippingCompanyDTO>.ErrorResponse("شركة الشحن غير موجودة");

            var dto = new ShippingCompanyDTO
            {
                ShippingCompanyId = company.ShippingCompanyId,
                Name = company.Name,
                ContactNumber = company.ContactNumber,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt
            };

            return ApiResponse<ShippingCompanyDTO>.SuccessResponse(dto);
        }

        // ============================
        // 3) Create
        // ============================
        public async Task<ApiResponse<ShippingCompanyDTO>> CreateAsync(CreateShippingCompanyDTO dto)
        {
            var company = new ShippingCompany
            {
                Name = dto.Name,
                ContactNumber = dto.ContactNumber,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.ShippingCompanies.Add(company);
            await _context.SaveChangesAsync();

            var result = new ShippingCompanyDTO
            {
                ShippingCompanyId = company.ShippingCompanyId,
                Name = company.Name,
                ContactNumber = company.ContactNumber,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt
            };

            return ApiResponse<ShippingCompanyDTO>.SuccessResponse(result, "تم إنشاء شركة الشحن بنجاح");
        }

        // ============================
        // 4) Update
        // ============================
        public async Task<ApiResponse<ShippingCompanyDTO>> UpdateAsync(int id, UpdateShippingCompanyDTO dto)
        {
            var company = await _context.ShippingCompanies.FindAsync(id);

            if (company == null)
                return ApiResponse<ShippingCompanyDTO>.ErrorResponse("شركة الشحن غير موجودة");

            company.Name = dto.Name;
            company.ContactNumber = dto.ContactNumber;
            company.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            var result = new ShippingCompanyDTO
            {
                ShippingCompanyId = company.ShippingCompanyId,
                Name = company.Name,
                ContactNumber = company.ContactNumber,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt
            };

            return ApiResponse<ShippingCompanyDTO>.SuccessResponse(result, "تم تحديث شركة الشحن بنجاح");
        }

        // ============================
        // 5) Delete
        // ============================
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var company = await _context.ShippingCompanies.FindAsync(id);

            if (company == null)
                return ApiResponse<bool>.ErrorResponse("شركة الشحن غير موجودة");

            _context.ShippingCompanies.Remove(company);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم حذف شركة الشحن");
        }
    }
}