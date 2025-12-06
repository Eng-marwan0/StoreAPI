using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAPI.Models
{
    public class UserAddress
    {
        [Key]
        public int AddressId { get; set; }

        public int UserId { get; set; }

        // ===== بيانات الشخص =====
        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = null!;

        // ===== بيانات الموقع =====
        [Required]
        [MaxLength(100)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Area { get; set; } = null!;   // مثال: زيونة، الكرادة

        [MaxLength(200)]
        public string? Region { get; set; }         // حي/محلة/منطقة فرعية

        [MaxLength(200)]
        public string? Street { get; set; }

        [MaxLength(200)]
        public string? Building { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }          // وصف إضافي لمكان السكن

        // ===== إحداثيات GPS =====
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ===== العلاقات =====
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        // كل الطلبات التي استخدمت هذا العنوان
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}