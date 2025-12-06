using System;
using System.Collections.Generic;

namespace StoreAPI.DTOs
{
    public class ShipmentDTO
    {
        public int ShipmentId { get; set; }
        public int OrderId { get; set; }

        public int ShippingCompanyId { get; set; }
        public string? ShippingCompanyName { get; set; }

        public string Status { get; set; } = "Pending";

        public string? TrackingNumber { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public List<ShipmentItemDTO> Items { get; set; } = new();
    }

    public class ShipmentItemDTO
    {
        public int ProductId { get; set; }
        public string? ProductNameAr { get; set; }
        public string? ProductNameEn { get; set; }

        public int Quantity { get; set; }

        public string? MainImageUrl { get; set; }
    }

    public class CreateShipmentDTO
    {
        public int OrderId { get; set; }
        public int ShippingCompanyId { get; set; }
        public string? TrackingNumber { get; set; }      // 👈 تمت إضافتها
    }

    public class UpdateShipmentStatusDTO
    {
        public string Status { get; set; } = "Pending";
        public string? TrackingNumber { get; set; }
    }
}