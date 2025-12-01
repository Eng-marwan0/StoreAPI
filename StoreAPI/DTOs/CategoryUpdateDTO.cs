namespace StoreAPI.DTOs
{
    public class CategoryUpdateDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public bool IsActive { get; set; }
        public IFormFile? Image { get; set; }
    }
}