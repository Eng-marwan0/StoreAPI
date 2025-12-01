using System.Collections.Generic;
using System.Threading.Tasks;
using StoreAPI.DTOs;
using StoreAPI.Helpers;

namespace StoreAPI.Services.Interfaces
{
    public interface IProductService
    {
        // جلب كل المنتجات
        Task<ApiResponse<List<ProductDTO>>> GetAllAsync();

        // جلب منتج واحد بالتعرّيف
        Task<ApiResponse<ProductDTO>> GetByIdAsync(int id);

        // إنشاء منتج جديد مع الصور
        Task<ApiResponse<ProductDTO>> CreateAsync(ProductCreateDTO dto);

        // تعديل منتج
        Task<ApiResponse<ProductDTO>> UpdateAsync(int id, ProductUpdateDTO dto);

        // حذف منتج
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}