namespace StoreAPI.DTOs
{
    public class AddToCartDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}