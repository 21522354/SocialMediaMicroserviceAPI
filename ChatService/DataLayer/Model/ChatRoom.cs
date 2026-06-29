namespace ChatService.DataLayer.Model
{
    public class ChatRoom
    {
        public Guid ChatRoomId { get; set; }
        public int FirstUserId { get; set; }
        public int SecondUserId { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; } 
    }
}
