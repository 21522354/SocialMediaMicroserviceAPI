using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UserService.DataLayer.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string NickName { get; set; }        
        public string FullName { get; set; }        
        public string Avatar { get; set; }
        public string FbId { get; set; }
    }
}
