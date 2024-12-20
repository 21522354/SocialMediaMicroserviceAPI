namespace PostService.Data_Layer.Models
{
    public class SeenReels
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }  
    }
}
