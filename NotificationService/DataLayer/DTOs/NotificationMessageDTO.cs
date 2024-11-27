namespace NotificationService.DataLayer.DTOs
{
    public class NotificationMessageDTO
    {
        public List<Guid> ListUserReceiveMessage { get; set; }
        public Guid UserInvoke { get; set; }
        public string Message { get; set; }
        public Guid PostId { get; set; }
        public Guid StoryId { get; set; }
        public Guid CommentId { get; set; } 
        public string EventType { get; set; }       
    }
}
