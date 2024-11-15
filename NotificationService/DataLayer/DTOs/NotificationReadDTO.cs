namespace NotificationService.DataLayer.DTOs
{
    public class NotificationReadDTO
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public Guid PostId { get; set; }
        public string EventType { get; set; }
    }
}
