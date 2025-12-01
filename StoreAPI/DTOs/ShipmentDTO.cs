namespace StoreAPI.DTOs
{
    public class ShipmentDTO
    {
        public int ShipmentId { get; set; }
        public int OrderId { get; set; }

        public int ShippingCompanyId { get; set; }
        public string? ShippingCompanyName { get; set; }

        public string Status { get; set; } = "";
        public string? TrackingNumber { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}