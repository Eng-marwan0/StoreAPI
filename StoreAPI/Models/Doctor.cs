using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

public partial class Doctor
{
    [Key]
    public int DoctorId { get; set; }

    public int ClinicId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    public string? Specialty { get; set; }

    public string? Bio { get; set; }

    public int? ExperienceYears { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Fee { get; set; }

    [StringLength(500)]
    public string? PhotoUrl { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Doctor")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [ForeignKey("ClinicId")]
    [InverseProperty("Doctors")]
    public virtual Clinic Clinic { get; set; } = null!;

    [InverseProperty("Doctor")]
    public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();
}
