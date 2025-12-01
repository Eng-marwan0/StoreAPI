using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace StoreAPI.Models
{
    public class UserAddress
    {
        [Key]
        public int AddressId { get; set; }

        public int UserId { get; set; }

        // ===========================
        // بيانات المستخدم
        // ===========================
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Phone { get; set; }

        // ===========================
        // بيانات الموقع
        // ===========================
        [Required]
        public string City { get; set; }

        [Required]
        public string Area { get; set; }       // كرادة - منصورية - زيونة...

        public string Region { get; set; }     // اختيارية
        public string Street { get; set; }
        public string Building { get; set; }

        public string Notes { get; set; }      // وصف إضافي

        // ===========================
        // إحداثيات GPS
        // ===========================
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ===========================
        // علاقات DB
        // ===========================
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // 👈 هذا الجزء الجديد الذي يحل المشكلة
        [InverseProperty("DeliveryAddress")]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}