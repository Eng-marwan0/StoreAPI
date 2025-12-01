using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

public partial class Offer
{
    [Key]
    public int OfferId { get; set; }

    [StringLength(200)]
    public string TitleAr { get; set; } = null!;

    [StringLength(200)]
    public string TitleEn { get; set; } = null!;

    public string? DescriptionAr { get; set; }

    public string? DescriptionEn { get; set; }

    [StringLength(20)]
    public string DiscountType { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("OfferId")]
    [InverseProperty("Offers")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
