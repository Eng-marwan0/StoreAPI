using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Models;

[Index("AdminId", Name = "IX_AdminLogs_AdminId")]
public partial class AdminLog
{
    [Key]
    public int LogId { get; set; }

    public int? AdminId { get; set; }

    [StringLength(200)]
    public string ActionType { get; set; } = null!;

    [StringLength(100)]
    public string? EntityName { get; set; }

    public int? EntityId { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("AdminId")]
    [InverseProperty("AdminLogs")]
    public virtual AdminUser? Admin { get; set; }
}
