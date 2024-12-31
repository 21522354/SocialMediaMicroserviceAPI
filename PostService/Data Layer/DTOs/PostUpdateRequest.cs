namespace PostService.Data_Layer.DTOs
{
    public class PostUpdateRequest
    {
        public Guid PostId { get; set; }        
        public Guid UserId { get; set; }
        public string PostTitle { get; set; }
        public IEnumerable<string> ImageAndVideo { get; set; }
        public IEnumerable<string> ListHagtag { get; set; }
    }
}
