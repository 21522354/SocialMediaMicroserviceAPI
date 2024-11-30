using ChatService.DataLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatService.DataLayer.Repository
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly ChatServiceDBContext _context;

        public ChatMessageRepository(ChatServiceDBContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ChatMessage message)
        {
            await _context.ChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ChatMessage message)
        {
            _context.ChatMessages.Remove(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ChatMessage>> GetAllChatByChatRoomId(Guid chatRoomId)
        {
            var listMessage = await _context.ChatMessages
                .Where(p => p.ChatRoomId == chatRoomId).Include(p => p.ChatRoom).OrderBy(p => p.SendDate).ToListAsync();
            return listMessage; 
        }

    }
}
