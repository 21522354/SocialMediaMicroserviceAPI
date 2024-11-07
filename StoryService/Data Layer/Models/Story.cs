namespace StoryService.Data_Layer.Models
{
    public class Story
    {
        public Guid StoryId { get; set; }
        public Guid UserId { get; set; }
        public string Image { get; set; }
        public string Sound { get; set; }
        public DateTime CreatedDate { get; set; }   
        public ICollection<UserAlreadySeenStory> UserAlreadySeenStories { get; set; }
    }
}
