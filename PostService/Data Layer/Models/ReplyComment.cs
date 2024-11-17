namespace PostService.Data_Layer.Models
{
    public class ReplyComment
    {
        public Guid ReplyCommentId { get; set; }
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
        public virtual PostComment Comment { get; set; }
        public Guid PostId { get; set; }        
        public virtual Post Post { get; set; }  
        public string Message { get; set; }
        public int NumberOfLike { get; set; }
    }
}
