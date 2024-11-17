namespace PostService.Data_Layer.DTOs
{
    public class ReplyCommentRequest
    {
        public Guid CommentId { get; set; }     
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }    
        public string Message { get; set; }     
    }
}
