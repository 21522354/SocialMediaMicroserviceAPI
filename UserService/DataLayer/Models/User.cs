using System.ComponentModel.DataAnnotations;

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
        [StringLength(50)]
        public string Name { get; set; }
        public string Image { get; set; }
        public string FbId { get; set; }
    }
}
