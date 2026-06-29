using ChatService.AsyncDataService;
using ChatService.Common;
using ChatService.DataLayer;
using ChatService.DataLayer.DTO;
using ChatService.DataLayer.Model;
using ChatService.SyncDataService;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Service
{
    public interface IChatService
    {
        Task<ResponseData<List<ChatRoomReadDTO>>> GetAllChatRoomForUser(int userId);
        Task<ResponseData<List<ChatMessageReadDTO>>> GetSpecificChatRoom(Guid chatRoomId);
        Task<ResponseData<string>> SendText(SendTextRequest request);
        Task<ResponseData<string>> DeleteChatRoom(Guid chatRoomId);
        Task<ResponseData<string>> SendMedias(SendMediaRequest request);
    }

    public class S_Chat : IChatService
    {
        private readonly ChatServiceDBContext _context;
        private readonly IUserDataClient _userDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public S_Chat(
            ChatServiceDBContext context,
            IUserDataClient userDataClient,
            IMessageBusClient messageBusClient)
        {
            _context = context;
            _userDataClient = userDataClient;
            _messageBusClient = messageBusClient;
        }

        public async Task<ResponseData<List<ChatRoomReadDTO>>> GetAllChatRoomForUser(int userId)
        {
            var res = new ResponseData<List<ChatRoomReadDTO>>();
            try
            {
                var user = await _userDataClient.GetUserById(userId);
                if (user == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this user";
                    return res;
                }

                var chatRooms = await _context.ChatRooms
                    .Where(p => p.FirstUserId == userId || p.SecondUserId == userId)
                    .ToListAsync();

                var listChatRoomReadDTO = new List<ChatRoomReadDTO>();
                foreach (var chatRoom in chatRooms)
                {
                    var friendId = userId == chatRoom.FirstUserId ? chatRoom.SecondUserId : chatRoom.FirstUserId;
                    var friend = await _userDataClient.GetUserById(friendId);
                    if (friend == null) continue;

                    var lastMessage = await _context.ChatMessages
                        .Where(p => p.ChatRoomId == chatRoom.ChatRoomId)
                        .OrderByDescending(p => p.SendDate)
                        .FirstOrDefaultAsync();

                    var chatRoomReadDTO = new ChatRoomReadDTO
                    {
                        ChatRoomId = chatRoom.ChatRoomId,
                        UserId = friend.UserId,
                        Avatar = friend.Avatar,
                        Name = friend.FullName,
                        IsOnline = false,
                    };

                    if (lastMessage != null)
                    {
                        chatRoomReadDTO.LastMessage = lastMessage.Message;
                        chatRoomReadDTO.LastMessageTime = lastMessage.SendDate;
                    }

                    listChatRoomReadDTO.Add(chatRoomReadDTO);
                }

                res.data = listChatRoomReadDTO.OrderByDescending(p => p.LastMessageTime).ToList();
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<List<ChatMessageReadDTO>>> GetSpecificChatRoom(Guid chatRoomId)
        {
            var res = new ResponseData<List<ChatMessageReadDTO>>();
            try
            {
                var chatRoom = await _context.ChatRooms.FindAsync(chatRoomId);
                if (chatRoom == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't found this ChatRoom";
                    return res;
                }

                var firstUser = await _userDataClient.GetUserById(chatRoom.FirstUserId);
                var secondUser = await _userDataClient.GetUserById(chatRoom.SecondUserId);
                if (firstUser == null || secondUser == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this user";
                    return res;
                }

                var listChatMessage = await _context.ChatMessages
                    .Where(p => p.ChatRoomId == chatRoomId)
                    .ToListAsync();

                var listChatMessageReadDTO = new List<ChatMessageReadDTO>();
                foreach (var chatMessage in listChatMessage)
                {
                    var user = chatMessage.UserSendId == firstUser.UserId ? firstUser : secondUser;
                    listChatMessageReadDTO.Add(new ChatMessageReadDTO
                    {
                        UserSendId = chatMessage.UserSendId,
                        Avatar = user.Avatar,
                        NickName = user.NickName,
                        ChatMessageId = chatMessage.ChatMessageId,
                        MediaLink = chatMessage.MediaLink,
                        Message = chatMessage.Message,
                        SendDate = chatMessage.SendDate,
                        Type = chatMessage.Type,
                    });
                }

                res.data = listChatMessageReadDTO.OrderBy(p => p.SendDate).ToList();
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<string>> SendText(SendTextRequest request)
        {
            var res = new ResponseData<string>();
            try
            {
                var firstUser = await _userDataClient.GetUserById(request.UserSendId);
                var secondUser = await _userDataClient.GetUserById(request.UserReceiveId);
                if (firstUser == null || secondUser == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this user";
                    return res;
                }

                var chatRoom = await GetOrCreateChatRoomAsync(request.UserSendId, request.UserReceiveId);
                await _context.ChatMessages.AddAsync(new ChatMessage
                {
                    ChatMessageId = Guid.NewGuid(),
                    ChatRoomId = chatRoom.ChatRoomId,
                    MediaLink = null,
                    Message = request.Message,
                    UserSendId = request.UserSendId,
                    SendDate = DateTime.UtcNow,
                    Type = "Text"
                });

                await _context.SaveChangesAsync();

                await PublishNotificationAsync(new NotificationMessageDTO
                {
                    UserId = request.UserReceiveId,
                    UserInvoke = request.UserSendId,
                    EventType = "NewMessage",
                    Message = $"{firstUser.NickName} sent you a message"
                });

                res.data = "Sent text successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<string>> DeleteChatRoom(Guid chatRoomId)
        {
            var res = new ResponseData<string>();
            try
            {
                var chatRoom = await _context.ChatRooms.FindAsync(chatRoomId);
                if (chatRoom == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this chat room";
                    return res;
                }

                _context.ChatRooms.Remove(chatRoom);
                await _context.SaveChangesAsync();

                res.data = "Delete chatroom successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<string>> SendMedias(SendMediaRequest request)
        {
            var res = new ResponseData<string>();
            try
            {
                var firstUser = await _userDataClient.GetUserById(request.UserSendId);
                var secondUser = await _userDataClient.GetUserById(request.UserReceiveId);
                if (firstUser == null || secondUser == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this user";
                    return res;
                }

                var chatRoom = await GetOrCreateChatRoomAsync(request.UserSendId, request.UserReceiveId);
                foreach (var img in request.Images ?? new List<string>())
                {
                    await _context.ChatMessages.AddAsync(new ChatMessage
                    {
                        ChatMessageId = Guid.NewGuid(),
                        ChatRoomId = chatRoom.ChatRoomId,
                        MediaLink = img,
                        Message = null,
                        UserSendId = request.UserSendId,
                        SendDate = DateTime.UtcNow,
                        Type = "Media"
                    });
                }

                await _context.SaveChangesAsync();

                await PublishNotificationAsync(new NotificationMessageDTO
                {
                    UserId = request.UserReceiveId,
                    UserInvoke = request.UserSendId,
                    EventType = "NewMessage",
                    Message = $"{firstUser.NickName} sent you a message"
                });

                res.data = "Sent media files successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        private async Task<ChatRoom> GetOrCreateChatRoomAsync(int firstUserId, int secondUserId)
        {
            var chatRoom = await _context.ChatRooms
                .FirstOrDefaultAsync(p =>
                    (p.FirstUserId == firstUserId && p.SecondUserId == secondUserId) ||
                    (p.FirstUserId == secondUserId && p.SecondUserId == firstUserId));

            if (chatRoom != null) return chatRoom;

            chatRoom = new ChatRoom
            {
                ChatRoomId = Guid.NewGuid(),
                FirstUserId = firstUserId,
                SecondUserId = secondUserId,
            };
            await _context.ChatRooms.AddAsync(chatRoom);

            return chatRoom;
        }

        private async Task PublishNotificationAsync(NotificationMessageDTO message)
        {
            try
            {
                await _messageBusClient.PublishNewNotification(message);
            }
            catch
            {
            }
        }
    }
}
