namespace PostService.Data_Layer.Models
{
    public class PostComment
    {
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }      
        public Guid CommentReplyId { get; set; }
        public string Message { get; set; }
        public int NumberOfLike { get; set; }       
    }
}
