namespace PostService.Data_Layer.DTOs
{
    public class UpdateCommentRequest
    {
        public int CommentId { get; set; }     
        public string Message { get; set; }
    }   
}
