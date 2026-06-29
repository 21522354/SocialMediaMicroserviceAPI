namespace ChatService.DataLayer.DTO
{
    public class SendMediaRequest
    {
        public int UserSendId { get; set; }
        public int UserReceiveId { get; set; }
        public List<string> Images { get; set; }    
    }
}
