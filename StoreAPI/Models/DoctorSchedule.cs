using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

public partial class DoctorSchedule
{
    [Key]
    public int ScheduleId { get; set; }

    public int DoctorId { get; set; }

    public byte DayOfWeek { get; set; }

    [Precision(0)]
    public TimeOnly StartTime { get; set; }

    [Precision(0)]
    public TimeOnly EndTime { get; set; }

    public int SlotDurationMinutes { get; set; }

    public int? MaxBookingsPerSlot { get; set; }

    [ForeignKey("DoctorId")]
    [InverseProperty("DoctorSchedules")]
    public virtual Doctor Doctor { get; set; } = null!;
}
