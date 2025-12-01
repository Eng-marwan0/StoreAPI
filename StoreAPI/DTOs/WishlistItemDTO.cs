namespace StoreAPI.DTOs
{
    public class WishlistItemDTO
    {
        public int ProductId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public decimal Price { get; set; }
        public string? MainImageUrl { get; set; }
        public bool InStock { get; set; }
    }
}