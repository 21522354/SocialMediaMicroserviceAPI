using ChatService.DataLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatService.DataLayer.Repository
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly ChatServiceDBContext _context;

        public ChatRoomRepository(ChatServiceDBContext context)
        {
            _context = context;
        }
        public async Task AddChatRoom(ChatRoom chatRoom)
        {
            await _context.AddAsync(chatRoom);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChatRoom(ChatRoom chatRoom)
        {
            _context.ChatRooms.Remove(chatRoom);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ChatRoom>> GetALlChatRoomForUser(Guid userId)
        {
            return await _context.ChatRooms.Where(
                p => p.FirstUserId == userId || p.SecondUserId == userId).ToListAsync();
        }

        public async Task<ChatRoom> GetChatRoomById(Guid chatRoomId)
        {
            return await _context.ChatRooms.FindAsync(chatRoomId); 
        }

        public async Task<ChatRoom> GetChatRoomByUserInChatRoom(Guid firstUserId, Guid secondUserId)
        {
            return await _context.ChatRooms.FirstOrDefaultAsync(
                p =>
                (p.FirstUserId == firstUserId && p.SecondUserId == secondUserId) ||
                (p.FirstUserId == secondUserId && p.SecondUserId == firstUserId)
                );
        }

    }
}
