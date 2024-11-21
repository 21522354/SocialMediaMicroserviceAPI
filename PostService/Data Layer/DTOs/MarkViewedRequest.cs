namespace PostService.Data_Layer.DTOs
{
    public class MarkViewedRequest
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }        
    }
}
