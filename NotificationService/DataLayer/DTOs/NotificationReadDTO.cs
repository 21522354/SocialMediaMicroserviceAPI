namespace NotificationService.DataLayer.DTOs
{
    public class NotificationReadDTO
    {
        public Guid UserId { get; set; }
        public Guid UserInvoke { get; set; }    
        public string Message { get; set; }
        public string CheckTag { get; set; }    
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }
        public Guid StoryId { get; set; }           
        public string EventType { get; set; }
    }
}
