using ChatService.DataLayer.Model;

namespace ChatService.DataLayer.Repository
{
    public interface IChatRoomRepository
    {
        Task AddChatRoom(ChatRoom chatRoom);    
        Task DeleteChatRoom(ChatRoom chatRoom); 
        Task<List<ChatRoom>> GetALlChatRoomForUser(Guid userId);    
        Task<ChatRoom> GetChatRoomById(Guid chatRoomId);
        Task<ChatRoom> GetChatRoomByUserInChatRoom(Guid firstUserId, Guid secondUserId);
    }
}
