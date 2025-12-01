using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

public partial class Booking
{
    [Key]
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int ClinicId { get; set; }

    public int DoctorId { get; set; }

    public DateOnly BookingDate { get; set; }

    [Precision(0)]
    public TimeOnly BookingTime { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [StringLength(50)]
    public string PaymentStatus { get; set; } = null!;

    [StringLength(50)]
    public string PaymentMethod { get; set; } = null!;

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("ClinicId")]
    [InverseProperty("Bookings")]
    public virtual Clinic Clinic { get; set; } = null!;

    [ForeignKey("DoctorId")]
    [InverseProperty("Bookings")]
    public virtual Doctor Doctor { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Bookings")]
    public virtual User User { get; set; } = null!;
}
