namespace StoreAPI.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public int? CategoryId { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }

        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public int Stock { get; set; }

        public string? MainImageUrl { get; set; }

        // روابط صور إضافية للمنتج
        public List<string> Images { get; set; } = new List<string>();
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}