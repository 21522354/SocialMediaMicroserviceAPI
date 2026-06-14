namespace PostService.Data_Layer.Models
{
    public class ReplyComment
    {
        public int ReplyCommentId { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public virtual PostComment Comment { get; set; }
        public int PostId { get; set; }        
        public virtual Post Post { get; set; }      
        public string Message { get; set; }
        public int NumberOfLike { get; set; }
    }
}
