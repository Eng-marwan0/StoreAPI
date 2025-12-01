namespace StoreAPI.DTOs
{
    public class OrderItemDTO
    {
        public int OrderItemId { get; set; }

        public int ProductId { get; set; }

        public string? ProductNameAr { get; set; }

        public string? ProductNameEn { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }   // 👈 الحل هنا
    }
}