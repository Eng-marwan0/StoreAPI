namespace StoreAPI.DTOs
{
    public class UserAddressDTO
    {
        public int AddressId { get; set; }
        public string? City { get; set; }
        public string? Area { get; set; }
        public string? Street { get; set; }
        public string? Notes { get; set; }
    }
}