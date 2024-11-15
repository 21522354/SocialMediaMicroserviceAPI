namespace PostService.Data_Layer.DTOs
{
    public class NotificationMessageDTO
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public Guid PostId { get; set; }
        public string EventType { get; set; }   
    }
}
