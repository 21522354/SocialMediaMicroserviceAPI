namespace PostService.Data_Layer.Models
{
    public class PostComment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }      
        public string Message { get; set; }
        public int NumberOfLike { get; set; }       
        public ICollection<ReplyComment> ReplyComments { get; set; }    
    }
}
