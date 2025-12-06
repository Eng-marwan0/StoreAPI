using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAPI.Models
{
    public class Shipment
    {
        [Key]
        public int ShipmentId { get; set; }

        public int OrderId { get; set; }

        public int ShippingCompanyId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ShippingCost { get; set; } = 0;   // 👈 تمت إضافتها هنا

        [StringLength(100)]
        public string? TrackingNumber { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending - Shipped - Delivered - Canceled

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeliveredAt { get; set; }

        // ===== العلاقات =====
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("ShippingCompanyId")]
        public virtual ShippingCompany ShippingCompany { get; set; }

        public virtual ICollection<ShipmentItem> Items { get; set; } = new List<ShipmentItem>();
    }
}