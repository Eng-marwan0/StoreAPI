using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

[Index("Code", Name = "UQ__Coupons__A25C5AA77B4A708A", IsUnique = true)]
public partial class Coupon
{
    [Key]
    public int CouponId { get; set; }

    [StringLength(50)]
    public string Code { get; set; } = null!;

    [StringLength(300)]
    public string? Description { get; set; }

    [StringLength(20)]
    public string DiscountType { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? DiscountValue { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? MinOrderAmount { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? MaxDiscountValue { get; set; }

    public int? UsageLimitPerUser { get; set; }

    public int? UsageLimitGlobal { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Coupon")]
    public virtual ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();

    [InverseProperty("Coupon")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
