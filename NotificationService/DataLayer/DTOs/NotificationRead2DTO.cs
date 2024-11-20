namespace NotificationService.DataLayer.DTOs
{
    public class NotificationRead2DTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Message { get; set; }
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }
        public Guid StoryId { get; set; }
        public string EventType { get; set; }
    }
}
