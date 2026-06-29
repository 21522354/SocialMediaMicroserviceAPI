namespace StoryService.Data_Layer.DTOs
{
    public class CreateUserAlreadySeenStoryRequest
    {
        public Guid StoryId { get; set; }
        public int UserId { get; set; }    
    }
}
