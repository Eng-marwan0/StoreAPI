using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs
{
    public class AddressDTO
    {
        public int AddressId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Area { get; set; } = string.Empty;

        public string? Region { get; set; }

        public string? Street { get; set; }

        public string? Building { get; set; }

        public string? Notes { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }

    public class CreateAddressDTO
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Area { get; set; } = string.Empty;

        public string? Region { get; set; }

        public string? Street { get; set; }

        public string? Building { get; set; }

        public string? Notes { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }

    public class UpdateAddressDTO
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Area { get; set; } = string.Empty;

        public string? Region { get; set; }

        public string? Street { get; set; }

        public string? Building { get; set; }

        public string? Notes { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}