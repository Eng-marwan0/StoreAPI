using StoreAPI.Data;
using StoreAPI.Models;

namespace StoreAPI.Data.Seed
{
    public static class ProductSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.Products.Any())
                return;

            var products = new List<Product>
            {
                new Product
                {
                    CategoryId = 1,
                    NameAr = "هاتف سامسونج S23",
                    NameEn = "Samsung S23",
                    DescriptionAr = "هاتف قوي بكاميرا ممتازة",
                    DescriptionEn = "Flagship phone with powerful camera",
                    Price = 900,
                    Stock = 50,
                    MainImageUrl = "uploads/products/main/s23.png",
                    CreatedAt = DateTime.UtcNow
                },

                new Product
                {
                    CategoryId = 2,
                    NameAr = "قميص رجالي",
                    NameEn = "Men Shirt",
                    DescriptionAr = "قميص أنيق عالي الجودة",
                    DescriptionEn = "High quality men's shirt",
                    Price = 20,
                    Stock = 150,
                    MainImageUrl = "uploads/products/main/shirt.png",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            // إضافة صور معرض المنتجات
            var gallery = new List<ProductImage>
            {
                new ProductImage { ProductId = 1, ImageUrl = "uploads/products/gallery/s23-1.png", IsMain = false },
                new ProductImage { ProductId = 1, ImageUrl = "uploads/products/gallery/s23-2.png", IsMain = false },
                new ProductImage { ProductId = 2, ImageUrl = "uploads/products/gallery/shirt-1.png", IsMain = false }
            };

            await context.ProductImages.AddRangeAsync(gallery);
            await context.SaveChangesAsync();
        }
    }
}