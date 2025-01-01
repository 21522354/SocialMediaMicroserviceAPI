using System.ComponentModel.DataAnnotations;

namespace PostService.Data_Layer.Models
{
    public class SavePost
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }  
    }
}
