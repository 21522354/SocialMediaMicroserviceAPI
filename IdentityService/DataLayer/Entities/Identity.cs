using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IdentityService.DataLayer.Models
{
    public class Identity
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        public string IdentityName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string RefreshToken { get; set; }   
        public string RefreshExpireTime { get; set; }   
        public bool IsLocked { get; set; }          
        public DateTime CreatedAt { get; set; }
    }       
}
