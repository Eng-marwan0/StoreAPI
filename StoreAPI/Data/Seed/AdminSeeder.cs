using StoreAPI.Data;
using StoreAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace StoreAPI.Data.Seed
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // لو يوجد أدمن مسبقاً.. لا تنشئ واحد جديد
            if (context.AdminUsers.Any())
                return;

            string HashPassword(string password)
            {
                using var sha = SHA256.Create();
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }

            var admin = new AdminUser
            {
                FullName = "Super Admin",
                Email = "admin@store.com",
                PasswordHash = HashPassword("Admin12345"), // كلمة مرور مشفّرة
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            context.AdminUsers.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}