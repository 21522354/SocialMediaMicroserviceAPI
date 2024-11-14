namespace NotificationService.DataLayer.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public Guid PostId { get; set; }        
        public bool IsAlreadySeen { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
