namespace StoryService.Data_Layer.DTOs
{
    public class CreateStoryRequest
    {
        public Guid UserId { get; set; }
        public string Image { get; set; }
        public string Sound { get; set; }
        public bool IsSaved { get; set; }       
    }
}
