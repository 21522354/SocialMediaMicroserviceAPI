namespace NotificationService.DataLayer.Models
{
    public class Notification
    {
        public Guid Id { get; set; }    
        public Guid UserId { get; set; }
        public Guid UserInvoke { get; set; }    
        public string Message { get; set; }
        public Guid PostId { get; set; }
        public Guid StoryId { get; set; }       
        public bool IsAlreadySeen { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
