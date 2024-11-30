namespace ChatService.DataLayer.DTO
{
    public class ChatRoomReadDTO
    {
        public Guid ChatRoomId { get; set; }
        public Guid UserId { get; set; }
        public string NickName { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public bool IsOnline { get; set; } = false;
    }
}
