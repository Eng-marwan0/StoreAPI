using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // ===============================
        // 1) Get All Products WITH pagination, sorting, filtering
        // ===============================
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProductDTO>>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] int? categoryId = null,
            [FromQuery] string? search = null,
            [FromQuery] string? sort = null)
        {
            var result = await _productService.GetAllAsync();

            var products = result.Data.AsQueryable();

            // --- filtering by category ---
            if (categoryId.HasValue)
                products = products.Where(p => p.CategoryId == categoryId);

            // --- search ---
            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(p =>
                    p.NameAr.Contains(search) ||
                    p.NameEn.Contains(search));
            }

            // --- sorting ---
            products = sort switch
            {
                "price_asc" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                "newest" => products.OrderByDescending(p => p.ProductId),
                _ => products.OrderByDescending(p => p.ProductId)
            };

            // --- pagination ---
            var paged = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return ApiResponse<List<ProductDTO>>.SuccessResponse(paged, "تم جلب المنتجات.");
        }

        // ===============================
        // 2) Product Details
        // ===============================
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDTO>>> GetById(int id)
        {
            return Ok(await _productService.GetByIdAsync(id));
        }

        // ===============================
        // 3) Products by Category
        // ===============================
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<ApiResponse<List<ProductDTO>>>> GetByCategory(int categoryId)
        {
            var all = await _productService.GetAllAsync();
            var filtered = all.Data.Where(p => p.CategoryId == categoryId).ToList();
            return ApiResponse<List<ProductDTO>>.SuccessResponse(filtered);
        }

        // ===============================
        // 4) Search
        // ===============================
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<List<ProductDTO>>>> Search(string text)
        {
            var all = await _productService.GetAllAsync();
            var filtered = all.Data.Where(p =>
                p.NameAr.Contains(text) ||
                p.NameEn.Contains(text)
            ).ToList();

            return ApiResponse<List<ProductDTO>>.SuccessResponse(filtered);
        }

        // ===============================
        // 5) Related Products
        // ===============================
        [HttpGet("{id}/related")]
        public async Task<ActionResult<ApiResponse<List<ProductDTO>>>> GetRelated(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (!product.Success)
                return ApiResponse<List<ProductDTO>>.ErrorResponse("المنتج غير موجود.");

            var all = await _productService.GetAllAsync();

            var related = all.Data
                .Where(p => p.CategoryId == product.Data.CategoryId && p.ProductId != id)
                .Take(10)
                .ToList();

            return ApiResponse<List<ProductDTO>>.SuccessResponse(related);
        }

        // ===============================
        // 6) Top Selling Products
        // ===============================
        [HttpGet("top-selling")]
        public async Task<ActionResult<ApiResponse<List<ProductDTO>>>> TopSelling()
        {
            var all = await _productService.GetAllAsync();
            var sorted = all.Data
                .OrderByDescending(p => p.Stock) // مؤقتاً لعدم وجود جدول مبيعات
                .Take(20)
                .ToList();

            return ApiResponse<List<ProductDTO>>.SuccessResponse(sorted);
        }

        // ===============================
        // 7) Latest Products
        // ===============================
        [HttpGet("latest")]
        public async Task<ActionResult<ApiResponse<List<ProductDTO>>>> Latest()
        {
            var all = await _productService.GetAllAsync();
            var sorted = all.Data
                .OrderByDescending(p => p.ProductId)
                .Take(20)
                .ToList();

            return ApiResponse<List<ProductDTO>>.SuccessResponse(sorted);
        }
    }
}