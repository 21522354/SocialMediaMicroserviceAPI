namespace PostService.Data_Layer.DTOs
{
    public class PostCommentReadDTO
    {
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Message { get; set; }
        public int NumberOfLike { get; set; }
        public IEnumerable<PostCommentReadDTO> ReplyComment { get; set; }       
    }
}
