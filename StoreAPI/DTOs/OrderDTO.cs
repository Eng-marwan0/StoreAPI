using StoreAPI.DTOs;

public class OrderDTO
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = null!;
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal TotalAmount { get; set; }

    public string PaymentMethod { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;
    public string Status { get; set; } = null!;

    public string? CouponCode { get; set; }
    public string? DeliveryStatus { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<OrderItemDTO> Items { get; set; } = new();
}