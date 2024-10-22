namespace PostService.Data_Layer.DTOs
{
    public class LikePostRequest
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }        
    }
}
