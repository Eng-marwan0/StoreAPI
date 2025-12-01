using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

[Index("OrderNumber", Name = "UQ__Orders__CAC5E743C9AF0F1F", IsUnique = true)]
public partial class Order
{
    [Key]
    public int OrderId { get; set; }

    public int AddressId { get; set; }   // 👈 VERY IMPORTANT

    [StringLength(50)]
    public string OrderNumber { get; set; } = null!;
    public int UserId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal ShippingFee { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal TotalAmount { get; set; }

    [StringLength(50)]
    public string PaymentMethod { get; set; } = null!;

    [StringLength(50)]
    public string PaymentStatus { get; set; } = null!;

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [StringLength(50)]
    public string? DeliveryStatus { get; set; }   // ✔️ هذا الصحيح

    public int? CouponId { get; set; }

    public int? DeliveryAddressId { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("CouponId")]
    [InverseProperty("Orders")]
    public virtual Coupon? Coupon { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();

    [ForeignKey("DeliveryAddressId")]
    [InverseProperty("Orders")]
    public virtual UserAddress? DeliveryAddress { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Order")]
    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();   // ✔️ يبقى كما هو

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User User { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
}