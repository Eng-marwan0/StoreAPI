using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs
{
    public class AddressDTO
    {
        public int AddressId { get; set; }
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Area { get; set; } = null!;
        public string? Region { get; set; }
        public string? Street { get; set; }
        public string? Building { get; set; }
        public string? Notes { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class CreateAddressDTO
    {
        [Required] public string FullName { get; set; } = null!;
        [Required] public string Phone { get; set; } = null!;
        [Required] public string City { get; set; } = null!;
        [Required] public string Area { get; set; } = null!;
        public string? Region { get; set; }
        public string? Street { get; set; }
        public string? Building { get; set; }
        public string? Notes { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class UpdateAddressDTO
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? Area { get; set; }
        public string? Region { get; set; }
        public string? Street { get; set; }
        public string? Building { get; set; }
        public string? Notes { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}