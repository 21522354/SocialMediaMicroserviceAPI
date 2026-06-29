namespace StoryService.Data_Layer.Models
{
    public class UserAlreadySeenStory
    {
        public int UserId { get; set; }
        public Guid StoryId { get; set; }   
        public virtual Story Story { get; set; }
    }
}
