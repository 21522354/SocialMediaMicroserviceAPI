namespace PostService.Controllers
{
    public class CommentPostRequest
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }         
    }
}
