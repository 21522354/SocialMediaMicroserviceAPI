namespace PostService.Data_Layer.DTOs
{
    public class UpdateCommentRequest
    {
        public Guid CommentId { get; set; }     
        public string Message { get; set; }
    }
}
