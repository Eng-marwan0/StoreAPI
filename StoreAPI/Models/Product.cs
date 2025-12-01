using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models
{
    public partial class Product
    {
        [Key]
        public int ProductId { get; set; }

        [StringLength(200)]
        public string? NameAr { get; set; }

        [StringLength(200)]
        public string? NameEn { get; set; }

        [StringLength(500)]
        public string? DescriptionAr { get; set; }

        [StringLength(500)]
        public string? DescriptionEn { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Price { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? OldPrice { get; set; }

        public int? Stock { get; set; }

        public string? MainImageUrl { get; set; }

        public int CategoryId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        // ====== العلاقات ======

        [ForeignKey("CategoryId")]
        [InverseProperty("Products")]
        public virtual Category Category { get; set; } = null!;

        [InverseProperty("Product")]
        public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

        [InverseProperty("Product")]
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        [InverseProperty("Product")]
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // 🔥 الحل الذي ينقصك
        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public virtual ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
    }
}