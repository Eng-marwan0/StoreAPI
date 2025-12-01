using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAPI.Models
{
    public class ProductImage
    {
        [Key]
        public int ImageId { get; set; }

        public int ProductId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        // 👇 الصورة الرئيسية أو لا
        public bool IsMain { get; set; } = false;

        // 👇 التاريخ المفقود والذي يحتاجه النظام
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}