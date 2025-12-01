using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;

namespace StoreAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse<List<CategoryDTO>>> GetAllAsync();
        Task<ApiResponse<CategoryDTO>> GetByIdAsync(int id);
        Task<ApiResponse<CategoryDTO>> CreateAsync(CategoryCreateDTO dto);
        Task<ApiResponse<CategoryDTO>> UpdateAsync(int id, CategoryUpdateDTO dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}