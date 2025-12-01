using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ================================
        // Get All
        // ================================
        public async Task<ApiResponse<List<CategoryDTO>>> GetAllAsync()
        {
            var categories = await _context.Categories
                .OrderByDescending(c => c.CategoryId)
                .Select(c => new CategoryDTO
                {
                    CategoryId = c.CategoryId,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn,
                    IsActive = c.IsActive ?? true,
                    ImageUrl = c.ImageUrl
                })
                .ToListAsync();

            return ApiResponse<List<CategoryDTO>>.SuccessResponse(categories, "تم جلب التصنيفات بنجاح.");
        }
        // ================================
        // Get By Id
        // ================================
        public async Task<ApiResponse<CategoryDTO>> GetByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return ApiResponse<CategoryDTO>.ErrorResponse("التصنيف غير موجود.");

            var dto = new CategoryDTO
            {
                CategoryId = category.CategoryId,
                NameAr = category.NameAr,
                NameEn = category.NameEn,
                IsActive = category.IsActive ?? true,
                ImageUrl = category.ImageUrl
            };

            return ApiResponse<CategoryDTO>.SuccessResponse(dto, "تم جلب التصنيف بنجاح.");
        }

        // ================================
        // CREATE
        // ================================
        public async Task<ApiResponse<CategoryDTO>> CreateAsync(CategoryCreateDTO dto)
        {
            string? imagePath = await ImageHelper.SaveImageAsync(dto.Image, _env);

            var category = new Category
            {
                NameAr = dto.NameAr,
                NameEn = dto.NameEn,
                ImageUrl = imagePath,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var result = new CategoryDTO
            {
                CategoryId = category.CategoryId,
                NameAr = category.NameAr,
                NameEn = category.NameEn,
                IsActive = category.IsActive ?? true,
                ImageUrl = category.ImageUrl
            };

            return ApiResponse<CategoryDTO>.SuccessResponse(result, "تم إنشاء التصنيف بنجاح.");
        }

        // ================================
        // UPDATE
        // ================================
        public async Task<ApiResponse<CategoryDTO>> UpdateAsync(int id, CategoryUpdateDTO dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return ApiResponse<CategoryDTO>.ErrorResponse("التصنيف غير موجود.");

            // تحديث البيانات الأساسية
            category.NameAr = dto.NameAr;
            category.NameEn = dto.NameEn;
            category.IsActive = dto.IsActive;

            // تحديث الصورة إن وُجدت
            if (dto.Image != null)
            {
                // حذف الصورة القديمة إن وُجدت
                if (!string.IsNullOrEmpty(category.ImageUrl))
                    ImageHelper.DeleteImage(category.ImageUrl, _env);

                // رفع الصورة الجديدة
                category.ImageUrl = await ImageHelper.SaveImageAsync(dto.Image, _env);
            }

            await _context.SaveChangesAsync();

            var result = new CategoryDTO
            {
                CategoryId = category.CategoryId,
                NameAr = category.NameAr,
                NameEn = category.NameEn,
                IsActive = category.IsActive ?? true,
                ImageUrl = category.ImageUrl
            };

            return ApiResponse<CategoryDTO>.SuccessResponse(result, "تم تحديث التصنيف بنجاح.");
        }

        // ================================
        // DELETE
        // ================================
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return ApiResponse<bool>.ErrorResponse("التصنيف غير موجود.");

            // حذف الصورة من السيرفر
            if (!string.IsNullOrEmpty(category.ImageUrl))
                ImageHelper.DeleteImage(category.ImageUrl, _env);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم حذف التصنيف بنجاح.");
        }
    }
}