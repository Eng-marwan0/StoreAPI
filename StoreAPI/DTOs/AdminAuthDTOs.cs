namespace StoreAPI.DTOs
{
    public class AdminLoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class AdminUserDTO
    {
        public int AdminId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = new();
        public string Token { get; set; } = null!;
    }
}