namespace PostService.Data_Layer.DTOs
{
    public class ReplyCommentRequest
    {
        public int CommentId { get; set; }     
        public int UserId { get; set; }
        public int PostId { get; set; }    
        public string Message { get; set; }         
    }
}
