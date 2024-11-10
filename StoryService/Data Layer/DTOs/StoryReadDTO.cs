namespace StoryService.Data_Layer.DTOs
{
    public class StoryReadDTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int Index { get; set; }      
        public List<StoryRead> ListStory { get; set; }
    }
    public class StoryRead
    {
        public Guid StoryId { get; set; }       
        public string Image { get; set; }
        public string Sound { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
