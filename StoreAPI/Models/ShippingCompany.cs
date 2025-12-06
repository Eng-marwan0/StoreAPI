using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAPI.Models
{
    public class ShippingCompany
    {
        [Key]
        public int ShippingCompanyId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        // رقم التواصل
        [StringLength(50)]
        public string? ContactNumber { get; set; }

        // هل الشركة فعالة؟
        public bool IsActive { get; set; } = true;

        // تاريخ الإنشاء
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // علاقة: الشركة لديها عدة شحنات
        public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}