using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

public partial class ShippingCompany
{
    [Key]
    public int ShippingCompanyId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? ApiBaseUrl { get; set; }

    [StringLength(500)]
    public string? ApiKey { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("ShippingCompany")]
    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}
