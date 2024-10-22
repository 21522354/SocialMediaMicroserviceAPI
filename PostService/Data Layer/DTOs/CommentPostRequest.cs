namespace PostService.Controllers
{
    public class CommentPostRequest
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }         
    }
}
