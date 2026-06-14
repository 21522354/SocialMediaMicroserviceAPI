namespace PostService.Data_Layer.DTOs
{
    public class PostReadDTO
    {
        public int PostId { get; set; }        
        public int UserId { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public string PostTitle { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<string> ImageAndVideo { get; set; }
        public IEnumerable<string> ListHagtags { get; set; }    
        public int NumberOfLike { get; set; }
        public int NumberOfComment { get; set; }
        public int NumberOfShare { get; set; }          

    }
}
