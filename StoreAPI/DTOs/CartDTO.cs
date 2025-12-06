using System.Collections.Generic;
using System.Linq;

namespace StoreAPI.DTOs
{
    // ============================================================
    // 1) DTO لإضافة منتج إلى السلة
    // ============================================================
    public class CartAddItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    // ============================================================
    // 2) DTO لتعديل عنصر داخل السلة
    // ============================================================
    public class CartUpdateItemDTO
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }

    // ============================================================
    // 3) عنصر من عناصر السلة
    // ============================================================
    public class CartItemDTO
    {
        public int CartItemId { get; set; }

        public int ProductId { get; set; }

        public string ProductNameAr { get; set; }
        public string ProductNameEn { get; set; }

        public string ImageUrl { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice => UnitPrice * Quantity;
    }

    // ============================================================
    // 4) السلة الكاملة
    // ============================================================
    public class CartDTO
    {
        public int CartId { get; set; }

        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();

        public int TotalItems => Items.Sum(i => i.Quantity);

        public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
    }
}