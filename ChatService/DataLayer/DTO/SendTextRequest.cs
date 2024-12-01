namespace ChatService.DataLayer.DTO
{
    public class SendTextRequest
    {
        public Guid UserSendId { get; set; }
        public Guid UserReceiveId { get; set; }
        public string Message { get; set; }     
    }
}
