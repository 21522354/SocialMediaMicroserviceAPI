using System.Reflection.Metadata.Ecma335;

namespace PostService.Data_Layer.Models
{
    public class PostMedia
    {
        public int Id { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }          
        public int STT { get; set; }
        public string Link { get; set; }
    }
}
