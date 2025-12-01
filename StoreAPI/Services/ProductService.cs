using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ===========================================
        // Get All Products
        // ===========================================
        public async Task<ApiResponse<List<ProductDTO>>> GetAllAsync()
        {
            var list = await _context.Products
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.ProductId)
                .Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    CategoryId = p.CategoryId,
                    NameAr = p.NameAr,
                    NameEn = p.NameEn,
                    DescriptionAr = p.DescriptionAr,
                    DescriptionEn = p.DescriptionEn,
                    Price = p.Price ?? 0,
                    OldPrice = p.OldPrice,
                    Stock = p.Stock ?? 0,
                    MainImageUrl = p.MainImageUrl,
                    Images = p.ProductImages.Select(i => i.ImageUrl).ToList()
                })
                .ToListAsync();

            return ApiResponse<List<ProductDTO>>.SuccessResponse(list, "تم جلب المنتجات بنجاح.");
        }

        // ===========================================
        // Get By ID
        // ===========================================
        public async Task<ApiResponse<ProductDTO>> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return ApiResponse<ProductDTO>.ErrorResponse("المنتج غير موجود.");

            var dto = new ProductDTO
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                NameAr = product.NameAr,
                NameEn = product.NameEn,
                DescriptionAr = product.DescriptionAr,
                DescriptionEn = product.DescriptionEn,
                Price = product.Price ?? 0,
                OldPrice = product.OldPrice,
                Stock = product.Stock ?? 0,
                MainImageUrl = product.MainImageUrl,
                Images = product.ProductImages.Select(i => i.ImageUrl).ToList()
            };

            return ApiResponse<ProductDTO>.SuccessResponse(dto);
        }

        // ===========================================
        // Create Product
        // ===========================================
        public async Task<ApiResponse<ProductDTO>> CreateAsync(ProductCreateDTO dto)
        {
            string? mainImageUrl = null;

            // ------- حفظ الصورة الرئيسية -------
            if (dto.MainImage != null)
                mainImageUrl = await ImageHelper.SaveImageAsync(dto.MainImage, _env);

            // ------- إنشاء المنتج -------
            var product = new Product
            {
                CategoryId = dto.CategoryId,
                NameAr = dto.NameAr,
                NameEn = dto.NameEn,
                DescriptionAr = dto.DescriptionAr,
                DescriptionEn = dto.DescriptionEn,
                Price = dto.Price,
                OldPrice = dto.OldPrice,
                Stock = dto.Stock,
                MainImageUrl = mainImageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // ------- صور المعرض -------
            if (dto.Images != null)
            {
                foreach (var img in dto.Images)
                {
                    string? url = await ImageHelper.SaveImageAsync(img, _env);

                    _context.ProductImages.Add(new ProductImage
                    {
                        ProductId = product.ProductId,
                        ImageUrl = url
                    });
                }

                await _context.SaveChangesAsync();
            }

            // ------- إخراج النتيجة -------
            var result = new ProductDTO
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                NameAr = product.NameAr,
                NameEn = product.NameEn,
                DescriptionAr = product.DescriptionAr,
                DescriptionEn = product.DescriptionEn,
                Price = product.Price ?? 0,
                OldPrice = product.OldPrice,
                Stock = product.Stock ?? 0,
                MainImageUrl = product.MainImageUrl,
                Images = _context.ProductImages
                    .Where(i => i.ProductId == product.ProductId)
                    .Select(i => i.ImageUrl).ToList()
            };

            return ApiResponse<ProductDTO>.SuccessResponse(result, "تم إنشاء المنتج بنجاح.");
        }

        // ===========================================
        // Update Product
        // ===========================================
        public async Task<ApiResponse<ProductDTO>> UpdateAsync(int id, ProductUpdateDTO dto)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return ApiResponse<ProductDTO>.ErrorResponse("المنتج غير موجود.");

            // تحديث البيانات الأساسية
            product.CategoryId = dto.CategoryId;
            product.NameAr = dto.NameAr;
            product.NameEn = dto.NameEn;
            product.DescriptionAr = dto.DescriptionAr;
            product.DescriptionEn = dto.DescriptionEn;
            product.Price = dto.Price;
            product.OldPrice = dto.OldPrice;
            product.Stock = dto.Stock;

            // تحديث الصورة الرئيسية
            if (dto.MainImage != null)
            {
                product.MainImageUrl = await ImageHelper.SaveImageAsync(dto.MainImage, _env);
            }

            // تحديث صور المعرض
            if (dto.Images != null)
            {
                // حذف الصور القديمة
                foreach (var img in product.ProductImages)
                {
                    ImageHelper.DeleteImage(img.ImageUrl, _env);
                }

                _context.ProductImages.RemoveRange(product.ProductImages);

                // إضافة الصور الجديدة
                foreach (var img in dto.Images)
                {
                    string? url = await ImageHelper.SaveImageAsync(img, _env);

                    _context.ProductImages.Add(new ProductImage
                    {
                        ProductId = product.ProductId,
                        ImageUrl = url
                    });
                }
            }

            await _context.SaveChangesAsync();

            // ---- بناء النتيجة ----
            var result = new ProductDTO
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                NameAr = product.NameAr,
                NameEn = product.NameEn,
                DescriptionAr = product.DescriptionAr,
                DescriptionEn = product.DescriptionEn,
                Price = product.Price ?? 0,
                OldPrice = product.OldPrice,
                Stock = product.Stock ?? 0,
                MainImageUrl = product.MainImageUrl,
                Images = _context.ProductImages
                    .Where(i => i.ProductId == product.ProductId)
                    .Select(i => i.ImageUrl).ToList()
            };

            return ApiResponse<ProductDTO>.SuccessResponse(result, "تم تحديث المنتج بنجاح.");
        }

        // ===========================================
        // Delete Product
        // ===========================================
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return ApiResponse<bool>.ErrorResponse("المنتج غير موجود.");

            // حذف الصور من السيرفر
            foreach (var img in product.ProductImages)
            {
                ImageHelper.DeleteImage(img.ImageUrl, _env);
            }

            // حذف سجلات الصور
            _context.ProductImages.RemoveRange(product.ProductImages);

            // حذف المنتج
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم حذف المنتج بنجاح.");
        }
    }
}