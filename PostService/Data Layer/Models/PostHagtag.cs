using System.ComponentModel.DataAnnotations;

namespace PostService.Data_Layer.Models
{
    public class PostHagtag
    {
        public int Id { get; set; }
        public Guid PostId { get; set; }    
        public virtual Post Post { get; set; }
        public string HagtagName { get; set; }      
    }
}
