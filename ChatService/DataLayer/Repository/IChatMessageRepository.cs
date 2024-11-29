using ChatService.DataLayer.Model;

namespace ChatService.DataLayer.Repository
{
    public interface IChatMessageRepository
    {
        Task AddAsync(ChatMessage message);
        Task DeleteAsync(ChatMessage message);      
        Task<List<ChatMessage>> GetAllChatByChatRoomId(Guid chatRoomId);
    }
}
