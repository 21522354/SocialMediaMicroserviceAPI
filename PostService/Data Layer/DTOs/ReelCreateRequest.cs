namespace PostService.Data_Layer.DTOs
{
    public class ReelCreateRequest
    {
        public Guid UserId { get; set; }
        public string PostTitle { get; set; }
        public string Video { get; set; }
        public IEnumerable<string> ListHagtag { get; set; }
    }
}
