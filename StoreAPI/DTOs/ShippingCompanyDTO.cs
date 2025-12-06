namespace StoreAPI.DTOs
{
    public class ShippingCompanyDTO
    {
        public int ShippingCompanyId { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateShippingCompanyDTO
    {
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateShippingCompanyDTO
    {
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
    }
}