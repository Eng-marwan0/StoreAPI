using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

public partial class Shipment
{
    [Key]
    public int ShipmentId { get; set; }

    public int OrderId { get; set; }

    public int ShippingCompanyId { get; set; }

    [StringLength(200)]
    public string? TrackingNumber { get; set; }

    [StringLength(100)]
    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? DeliveredAt { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Shipments")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ShippingCompanyId")]
    [InverseProperty("Shipments")]
    public virtual ShippingCompany ShippingCompany { get; set; } = null!;

    public string? TrackingUrl { get; set; }
    public string? ExternalId { get; set; } // رقم الشحنة داخل API
    public decimal? ShippingCost { get; set; }
    public DateTime UpdatedAt { get; set; }
}
