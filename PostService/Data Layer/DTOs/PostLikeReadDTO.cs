namespace PostService.Data_Layer.DTOs
{
    public class PostLikeReadDTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }      
    }
}
