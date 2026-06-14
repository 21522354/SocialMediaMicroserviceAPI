namespace PostService.Data_Layer.Models
{
    public class PostLike
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }          
        public int UserId { get; set; }     
    }
}
