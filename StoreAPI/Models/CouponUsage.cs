using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

public partial class CouponUsage
{
    [Key]
    public int CouponUsageId { get; set; }

    public int CouponId { get; set; }

    public int UserId { get; set; }

    public int OrderId { get; set; }

    public DateTime UsedAt { get; set; }

    [ForeignKey("CouponId")]
    [InverseProperty("CouponUsages")]
    public virtual Coupon Coupon { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("CouponUsages")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("CouponUsages")]
    public virtual User User { get; set; } = null!;
}
