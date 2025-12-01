using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

[Index("Name", Name = "UQ__Roles__737584F68F1B470B", IsUnique = true)]
public partial class Role
{
    [Key]
    public int RoleId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(300)]
    public string? Remarks { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Roles")]
    public virtual ICollection<AdminUser> Admins { get; set; } = new List<AdminUser>();
}
