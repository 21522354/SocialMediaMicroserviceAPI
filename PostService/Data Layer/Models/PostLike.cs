namespace PostService.Data_Layer.Models
{
    public class PostLike
    {
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }          
        public Guid UserId { get; set; }    
    }
}
