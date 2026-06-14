namespace PostService.Data_Layer.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string PostTitle { get; set; }
        public DateTime CreatedDate { get; set; }
        public int NumberOfShare { get; set; }
        public bool IsReel { get; set; }        
        public ICollection<PostComment> PostComments { get; set; }      
        public ICollection<PostLike> PostLikes { get; set; }
        public ICollection<PostMedia> PostMedias { get; set; }
        public ICollection<UnseenPost> UnseenPosts { get; set; }    
        public ICollection<ReplyComment> ReplyComments { get; set; }
        public ICollection<PostHagtag> PostHagtags { get; set; }        
    }
}
