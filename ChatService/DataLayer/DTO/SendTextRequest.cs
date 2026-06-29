namespace ChatService.DataLayer.DTO
{
    public class SendTextRequest
    {
        public int UserSendId { get; set; }
        public int UserReceiveId { get; set; }
        public string Message { get; set; }     
    }
}
