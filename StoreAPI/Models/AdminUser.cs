using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

[Index("Email", Name = "UQ__AdminUse__A9D1053492F476B3", IsUnique = true)]
public partial class AdminUser
{
    [Key]
    public int AdminId { get; set; }

    [StringLength(150)]
    public string FullName { get; set; } = null!;

    [StringLength(200)]
    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Admin")]
    public virtual ICollection<AdminLog> AdminLogs { get; set; } = new List<AdminLog>();

    [ForeignKey("AdminId")]
    [InverseProperty("Admins")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
