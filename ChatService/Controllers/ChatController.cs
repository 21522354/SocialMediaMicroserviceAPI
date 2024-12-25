using ChatService.AsyncDataService;
using ChatService.DataLayer;
using ChatService.DataLayer.DTO;
using ChatService.DataLayer.Model;
using ChatService.DataLayer.Repository;
using ChatService.SyncDataService;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IUserDataClient _userDataClient;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;
        private readonly ChatServiceDBContext _context;

        public ChatController(IChatRoomRepository chatRoomRepository,
            IChatMessageRepository chatMessageRepository,
            IUserDataClient userDataClient,
            IMapper mapper,
            IMessageBusClient messageBusClient,
            ChatServiceDBContext context)
        {
            _chatRoomRepository = chatRoomRepository;
            _chatMessageRepository = chatMessageRepository;
            _userDataClient = userDataClient;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
            _context = context;
        }
        [HttpGet("users/{userId}/chatRooms")]
        public async Task<IActionResult> GetAllChatRoomForUser(Guid userId)
        {
            var user = await _userDataClient.GetUserById(userId);   
            if(user == null)
            {
                return BadRequest("Can't find this user");
            }

            var chatRooms = await _context.ChatRooms.Where(
                p => p.FirstUserId == userId || p.SecondUserId == userId).ToListAsync();
            var listChatRoomReadDTO = new List<ChatRoomReadDTO>();
            foreach (var chatRoom in chatRooms)
            {
                var userID = userId == chatRoom.FirstUserId ? chatRoom.SecondUserId : chatRoom.FirstUserId;
                var friend = await _userDataClient.GetUserById(userID);
                if (friend == null)
                {
                    return BadRequest("Can't find this user");
                }
                var lastMessage = await _context.ChatMessages.Where(p => p.ChatRoomId == chatRoom.ChatRoomId).OrderByDescending(p => p.SendDate).FirstOrDefaultAsync();
                var chatRoomReadDTO = new ChatRoomReadDTO()
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
            listChatRoomReadDTO = listChatRoomReadDTO.OrderByDescending(p => p.LastMessageTime).ToList();
            return Ok(listChatRoomReadDTO);
        }
        [HttpGet("chatRooms/{chatRoomId}")]
        public async Task<IActionResult> GetSpecificChatRoom(Guid chatRoomId)
        {
            var listChatMessage = await _context.ChatMessages.Where(p => p.ChatRoomId == chatRoomId).ToListAsync();
            var listChatMessageReadDTO = new List<ChatMessageReadDTO>();
            var chatRoom = await _context.ChatRooms.FindAsync(chatRoomId);
            if (chatRoom == null)
            {
                return BadRequest("Can't found this ChatRoom");
            }
            var firstUser = await _userDataClient.GetUserById(chatRoom.FirstUserId);
            var secondUser = await _userDataClient.GetUserById(chatRoom.SecondUserId);
            foreach (var chatMessage in listChatMessage)
            {
                var user = chatMessage.UserSendId == firstUser.UserId ? firstUser : secondUser;
                var chatMessageReadDTO = new ChatMessageReadDTO()
                {
                    UserSendId = chatMessage.UserSendId,
                    Avatar = user.Avatar,
                    ChatMessageId = chatMessage.ChatMessageId,
                    MediaLink = chatMessage.MediaLink,
                    Message = chatMessage.Message,
                    SendDate = DateTime.Now,
                    Type = chatMessage.Type,
                };
                listChatMessageReadDTO.Add(chatMessageReadDTO);
            }
            return Ok(listChatMessageReadDTO);
        }
        [HttpPost("sendText")]
        public async Task<IActionResult> SendText(SendTextRequest request)
        {
            var firstUser = await _userDataClient.GetUserById(request.UserSendId);
            var secondUser = await _userDataClient.GetUserById(request.UserReceiveId);
            if (firstUser == null || secondUser == null)
            {
                return BadRequest("Can't find this user");
            }
            var chatRoom = await _context.ChatRooms.Where(
                p => p.FirstUserId == request.UserSendId && p.SecondUserId == request.UserReceiveId ||
                    p.FirstUserId == request.UserReceiveId && p.SecondUserId == request.UserSendId).FirstOrDefaultAsync();
            if (chatRoom == null)
            {
                chatRoom = new ChatRoom()
                {
                    ChatRoomId = Guid.NewGuid(),
                    FirstUserId = request.UserSendId,
                    SecondUserId = request.UserReceiveId,
                };
                await _context.ChatRooms.AddAsync(chatRoom);
            }
            var chatMessage = new ChatMessage()
            {
                ChatMessageId = Guid.NewGuid(),
                ChatRoomId = chatRoom.ChatRoomId,
                MediaLink = null,
                Message = request.Message,
                UserSendId = request.UserSendId,
                SendDate = DateTime.UtcNow,
                Type = "Text"
            };
            await _context.ChatMessages.AddAsync(chatMessage);
            await _context.SaveChangesAsync();

            await _messageBusClient.PublishNewNotification(new NotificationMessageDTO()
            {
                UserId = request.UserReceiveId,
                UserInvoke = request.UserSendId,
                EventType = "NewMessage",
                Message = $"{firstUser.NickName} sent you a message"
            });

            Console.WriteLine("Published new notification to message bus");

            return Ok("Sent text successfully"); 
        }
        [HttpPost("sendMedia")]
        public async Task<IActionResult> SendMedias(SendMediaRequest request)
        {
            var firstUser = await _userDataClient.GetUserById(request.UserSendId);
            var secondUser = await _userDataClient.GetUserById(request.UserReceiveId);
            if (firstUser == null || secondUser == null)
            {
                return BadRequest("Can't find this user");
            }
            var chatRoom = await _context.ChatRooms.Where(
                p => p.FirstUserId == request.UserSendId && p.SecondUserId == request.UserReceiveId ||
                    p.FirstUserId == request.UserReceiveId && p.SecondUserId == request.UserSendId).FirstOrDefaultAsync();
            if (chatRoom == null)
            {
                chatRoom = new ChatRoom()
                {
                    ChatRoomId = Guid.NewGuid(),
                    FirstUserId = request.UserSendId,
                    SecondUserId = request.UserReceiveId,
                };
                await _context.ChatRooms.AddAsync(chatRoom);
            }
            foreach (var img in request.Images)
            {
                var chatMessage = new ChatMessage()
                {
                    ChatMessageId = Guid.NewGuid(),
                    ChatRoomId = chatRoom.ChatRoomId,
                    MediaLink = img,
                    Message = null,
                    UserSendId = request.UserSendId,
                    SendDate = DateTime.UtcNow,
                    Type = "Media"
                };
                await _context.ChatMessages.AddAsync(chatMessage);
            }
            await _context.SaveChangesAsync();

            await _messageBusClient.PublishNewNotification(new NotificationMessageDTO()
            {
                UserId = request.UserReceiveId,
                UserInvoke = request.UserSendId,
                EventType = "NewMessage",
                Message = $"{firstUser.NickName} sent you a message"
            });

            Console.WriteLine("Published new notification to message bus");

            return Ok("Sent media files successfully");
        }
    }
}
