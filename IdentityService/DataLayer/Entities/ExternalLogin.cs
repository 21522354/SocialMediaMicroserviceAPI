using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.DataLayer.Entities;

[Table("ExternalLogin")]
[Index("Provider", "ProviderUserId", Name = "IX_ExternalLogins_Provider_User", IsUnique = true)]
public partial class ExternalLogin
{
    [Key]
    public int Id { get; set; }

    public int IdentityId { get; set; }

    [StringLength(50)]
    public string Provider { get; set; } = null!;

    [StringLength(255)]
    public string ProviderUserId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [ForeignKey("IdentityId")]
    [InverseProperty("ExternalLogins")]
    public virtual Identity Identity { get; set; } = null!;
}
