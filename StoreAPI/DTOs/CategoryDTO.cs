namespace StoreAPI.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
    }
}