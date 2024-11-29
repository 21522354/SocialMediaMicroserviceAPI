namespace ChatService.DataLayer.Model
{
    public class ChatRoom
    {
        public Guid ChatRoomId { get; set; }
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; } 
    }
}
