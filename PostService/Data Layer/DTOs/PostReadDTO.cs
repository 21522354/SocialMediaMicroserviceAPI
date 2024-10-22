namespace PostService.Data_Layer.DTOs
{
    public class PostReadDTO
    {
        public Guid PostId { get; set; }        
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string PostTitle { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<string> ImageAndVideo { get; set; }
        public int NumberOfLike { get; set; }
        public int NumberOfComment { get; set; }
        public int NumberOfShare { get; set; }      

    }
}
