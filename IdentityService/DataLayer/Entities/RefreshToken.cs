using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.DataLayer.Entities;

[Table("RefreshToken")]
[Index("IdentityId", Name = "IX_RefreshToken_IdentityId")]
[Index("TokenHash", Name = "IX_RefreshTokens_TokenHash", IsUnique = true)]
public partial class RefreshToken
{
    [Key]
    public int Id { get; set; }

    public int IdentityId { get; set; }

    [StringLength(256)]
    public string TokenHash { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    [StringLength(45)]
    public string? CreatedByIp { get; set; }

    [StringLength(45)]
    public string? RevokedByIp { get; set; }

    [StringLength(200)]
    public string? DeviceName { get; set; }

    [StringLength(1000)]
    public string? UserAgent { get; set; }

    [ForeignKey("IdentityId")]
    [InverseProperty("RefreshTokens")]
    public virtual Identity Identity { get; set; } = null!;
}
