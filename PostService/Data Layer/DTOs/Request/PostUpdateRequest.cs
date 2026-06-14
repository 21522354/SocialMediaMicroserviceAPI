namespace PostService.Data_Layer.DTOs
{
    public class PostUpdateRequest
    {
        public int PostId { get; set; }        
        public int UserId { get; set; }
        public string PostTitle { get; set; }
        public IEnumerable<string> ImageAndVideo { get; set; }
        public IEnumerable<string> ListHagtag { get; set; }
    }   
}
