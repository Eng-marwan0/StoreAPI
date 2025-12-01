using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

public partial class Category
{
    [Key]
    public int CategoryId { get; set; }

    public int? ParentId { get; set; }

    [StringLength(200)]
    public string NameAr { get; set; } = null!;

    [StringLength(200)]
    public string NameEn { get; set; } = null!;

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    public bool? IsActive { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Parent")]
    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual Category? Parent { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
