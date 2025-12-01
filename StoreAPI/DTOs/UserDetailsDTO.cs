namespace StoreAPI.DTOs
{
    public class UserDetailsDTO
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<UserAddressDTO>? Addresses { get; set; }

        public int TotalOrders { get; set; }
        public DateTime? LastOrderDate { get; set; }
    }
}