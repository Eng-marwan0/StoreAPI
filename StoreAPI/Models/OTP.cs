using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

[Table("OTP")]
public partial class OTP
{
    [Key]
    public int OtpId { get; set; }

    [StringLength(20)]
    public string Phone { get; set; } = null!;

    [StringLength(10)]
    public string Code { get; set; } = null!;

    [StringLength(50)]
    public string Purpose { get; set; } = null!;

    public int Attempts { get; set; }

    public bool IsUsed { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }
}
