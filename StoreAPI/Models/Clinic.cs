using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

public partial class Clinic
{
    [Key]
    public int ClinicId { get; set; }

    [StringLength(200)]
    public string NameAr { get; set; } = null!;

    [StringLength(200)]
    public string NameEn { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(300)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Clinic")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("Clinic")]
    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
