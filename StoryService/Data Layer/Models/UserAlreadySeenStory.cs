namespace StoryService.Data_Layer.Models
{
    public class UserAlreadySeenStory
    {
        public Guid UserId { get; set; }
        public Guid StoryId { get; set; }   
        public virtual Story Story { get; set; }
    }
}
