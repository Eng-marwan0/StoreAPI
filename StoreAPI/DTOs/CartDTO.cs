using System.Collections.Generic;

namespace StoreAPI.DTOs
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public List<CartItemDTO> Items { get; set; } = new();
        public int TotalItems { get; set; }
        public decimal SubTotal { get; set; }
    }
}