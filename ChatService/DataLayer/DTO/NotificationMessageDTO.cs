namespace ChatService.DataLayer.DTO
{
    public class NotificationMessageDTO
    {
        public int UserId { get; set; }
        public int UserInvoke { get; set; }
        public string Message { get; set; }
        public string CheckTag { get; set; }
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }
        public string EventType { get; set; }
    }
}
