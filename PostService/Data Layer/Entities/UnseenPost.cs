namespace PostService.Data_Layer.Models
{
    public class UnseenPost
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }      
    }
}
