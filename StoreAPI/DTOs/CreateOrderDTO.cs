namespace StoreAPI.DTOs;
public class CreateOrderDTO
{
    public int UserId { get; set; }
    public int AddressId { get; set; }
    public string PaymentMethod { get; set; } = "Cash";
    public string? CouponCode { get; set; }
    public string? Notes { get; set; }
    public int? ShippingCompanyId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}