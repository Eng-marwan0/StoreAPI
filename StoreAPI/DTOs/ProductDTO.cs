namespace StoreAPI.DTOs
{
    // =============================
    // Read DTO (Return to user)
    // =============================
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public int Stock { get; set; }
        public string? MainImageUrl { get; set; }
        public List<string>? Images { get; set; }
    }

    // =============================
    // Create Product DTO
    // =============================
    public class ProductCreateDTO
    {
        public int CategoryId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public int Stock { get; set; }

        public IFormFile? MainImage { get; set; }   // صورة رئيسية
        public List<IFormFile>? Images { get; set; } // صور معرض
    }

    // =============================
    // Update Product DTO
    // =============================
    public class ProductUpdateDTO
    {
        public int CategoryId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public int Stock { get; set; }

        public IFormFile? MainImage { get; set; }   // اختيارية
        public List<IFormFile>? Images { get; set; } // اختيارية
    }
}