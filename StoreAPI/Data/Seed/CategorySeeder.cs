using StoreAPI.Data;
using StoreAPI.Models;

namespace StoreAPI.Data.Seed
{
    public static class CategorySeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.Categories.Any())
                return;

            var categories = new List<Category>
            {
                new Category { NameAr = "إلكترونيات", NameEn = "Electronics", ImageUrl = "uploads/categories/electronics.png", IsActive = true },
                new Category { NameAr = "أزياء", NameEn = "Fashion", ImageUrl = "uploads/categories/fashion.png", IsActive = true },
                new Category { NameAr = "أجهزة منزلية", NameEn = "Home Appliances", ImageUrl = "uploads/categories/home.png", IsActive = true }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }
    }
}