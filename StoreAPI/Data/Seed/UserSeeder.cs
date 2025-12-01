using StoreAPI.Data;
using StoreAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace StoreAPI.Data.Seed
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.Users.Any())
                return;

            string Hash(string password)
            {
                using var sha = SHA256.Create();
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }

            var users = new List<User>
            {
                new User
                {
                    FullName = "User One",
                    Email = "user1@test.com",
                    Phone = "07700000001",
                    PasswordHash = Hash("123456"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    FullName = "User Two",
                    Email = "user2@test.com",
                    Phone = "07700000002",
                    PasswordHash = Hash("123456"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }
}