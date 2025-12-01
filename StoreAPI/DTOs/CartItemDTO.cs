namespace StoreAPI.DTOs
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }

        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? MainImageUrl { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}