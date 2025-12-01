using Microsoft.AspNetCore.Http;

namespace StoreAPI.Helpers
{
    public static class ImageHelper
    {
        private static readonly string _folderName = "uploads";

        public static async Task<string> SaveImageAsync(IFormFile file, IWebHostEnvironment env)
        {
            if (file == null || file.Length == 0)
                return null;

            string uploadsPath = Path.Combine(env.WebRootPath, _folderName);

            // إنشاء المجلد إذا لم يكن موجوداً
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // مسار الصورة الذي سيتم تخزينه في القاعدة
            return $"/{_folderName}/{fileName}";
        }

        public static bool DeleteImage(string imagePath, IWebHostEnvironment env)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return false;

            string fullPath = Path.Combine(env.WebRootPath, imagePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }
    }
}