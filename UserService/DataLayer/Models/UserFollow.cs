using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.DataLayer.Models
{
    public class UserFollow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("UserFromId")]
        public Guid UserFromId { get; set; }
        public virtual User UserFrom { get; set; }
        [ForeignKey("UserToId")] 
        public Guid UserToId { get; set; }       
        public virtual User UserTo { get; set; }            
    }
}
