namespace PostService.Data_Layer.DTOs
{
    public class NotificationMessageDTO
    {
        public int UserId { get; set; }
        public int UserInvoke { get; set; }        
        public string Message { get; set; }
        public string CheckTag { get; set; }
        public int PostId { get; set; }
        public int CommentId { get; set; }     
        public string EventType { get; set; }   
    }   
}
