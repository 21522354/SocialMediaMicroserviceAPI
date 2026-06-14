namespace PostService.Data_Layer.DTOs
{
    public class PostCommentReadDTO
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }  
        public string Message { get; set; }
        public int NumberOfLike { get; set; }
        public IEnumerable<PostCommentReadDTO> ReplyComment { get; set; }       
    }
}
