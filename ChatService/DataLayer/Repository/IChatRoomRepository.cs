using ChatService.DataLayer.Model;

namespace ChatService.DataLayer.Repository
{
    public interface IChatRoomRepository
    {
        Task AddChatRoom(ChatRoom chatRoom);    
        Task DeleteChatRoom(ChatRoom chatRoom); 
        Task<List<ChatRoom>> GetALlChatRoomForUser(int userId);    
        Task<ChatRoom> GetChatRoomById(Guid chatRoomId);
        Task<ChatRoom> GetChatRoomByUserInChatRoom(int firstUserId, int secondUserId);
    }
}
