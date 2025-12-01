namespace StoreAPI.DTOs
{
    public class CategoryCreateDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public IFormFile? Image { get; set; }
    }
}