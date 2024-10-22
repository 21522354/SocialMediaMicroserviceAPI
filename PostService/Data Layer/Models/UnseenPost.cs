namespace PostService.Data_Layer.Models
{
    public class UnseenPost
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public bool IsAlreadySeen { get; set; }
        public virtual Post Post { get; set; }  
    }
}
