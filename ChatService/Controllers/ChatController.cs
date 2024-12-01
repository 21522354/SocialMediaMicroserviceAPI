using ChatService.AsyncDataService;
using ChatService.DataLayer.DTO;
using ChatService.DataLayer.Model;
using ChatService.DataLayer.Repository;
using ChatService.SyncDataService;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        public ChatController(IChatRoomRepository chatRoomRepository,
            IChatMessageRepository chatMessageRepository,
            IUserDataClient userDataClient,
            IMapper mapper,
            IMessageBusClient messageBusClient)
        {
            _chatRoomRepository = chatRoomRepository;
            _chatMessageRepository = chatMessageRepository;
            _userDataClient = userDataClient;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }
        [HttpGet("users/{userId}/chatRooms")]
        public async Task<IActionResult> GetAllChatRoomForUser(Guid userId)
        {
            var user = await _userDataClient.GetUserById(userId);   
            if(user == null)
            {
                return BadRequest("Can't find this user");
            }

            var chatRooms = await _chatRoomRepository.GetALlChatRoomForUser(userId);
            var listChatRoomReadDTO = new List<ChatRoomReadDTO>();
            foreach (var room in chatRooms)
            {
                var friendId = userId == room.FirstUserId ? room.SecondUserId : room.FirstUserId;
                var friendReadDTO = await _userDataClient.GetUserById(friendId);
                var chatRoomReadDTO = (friendReadDTO, room).Adapt<ChatRoomReadDTO>();
                listChatRoomReadDTO.Add(chatRoomReadDTO);
            }

            return Ok(listChatRoomReadDTO); 
        }
        [HttpGet("chatRooms/{chatRoomId}")]
        public async Task<IActionResult> GetSpecificChatRoom(Guid chatRoomId)
        {
            var chatMessages = await _chatMessageRepository.GetAllChatByChatRoomId(chatRoomId); 
            var chatRoom = await _chatRoomRepository.GetChatRoomById(chatRoomId);
            var firstUserReadDTO = await _userDataClient.GetUserById(chatRoom.FirstUserId);
            var secondUserReadDTO = await _userDataClient.GetUserById(chatRoom.SecondUserId);
            var listChatMessageReadDTO = new List<ChatMessageReadDTO>();    
            foreach (var chatMessage in chatMessages)
            {
                var userSend = chatMessage.UserId == firstUserReadDTO.UserId ? firstUserReadDTO : secondUserReadDTO;
                listChatMessageReadDTO.Add(_mapper.Map<ChatMessageReadDTO>((userSend, chatMessage)));
            }
            return Ok(listChatMessageReadDTO);  
        }
        [HttpPost("sendText")]
        public async Task<IActionResult> SendText(SendTextRequest request)
        {
            var chatRoom = await _chatRoomRepository.GetChatRoomByUserInChatRoom(request.UserSendId, request.UserReceiveId);
            if (chatRoom == null)
            {
                chatRoom = new ChatRoom() { ChatRoomId = Guid.NewGuid(), FirstUserId = request.UserSendId, SecondUserId = request.UserReceiveId };
                await _chatRoomRepository.AddChatRoom(chatRoom);    
            }

            var userInvoke = await _userDataClient.GetUserById(request.UserSendId);

            var chatMessage = new ChatMessage()
            {
                ChatMessageId = Guid.NewGuid(),
                ChatRoomId = chatRoom.ChatRoomId,
                Message = request.Message,
                UserId = request.UserSendId,
                SendDate = DateTime.Now,
                Type = "Text"
            };
            await _chatMessageRepository.AddAsync(chatMessage);

            await _messageBusClient.PublishNewNotification(new NotificationMessageDTO()
            {
                UserId = request.UserReceiveId,
                UserInvoke = request.UserSendId,
                EventType = "NewMessage",
                Message = $"{userInvoke.NickName} sent you a message"
            });

            Console.WriteLine("Published new notification to message bus");

            return Ok("Sent text successfully"); 
        }
        [HttpPost("sendMedia")]
        public async Task<IActionResult> SendMedias(SendMediaRequest request)
        {
            var chatRoom = await _chatRoomRepository.GetChatRoomByUserInChatRoom(request.UserSendId, request.UserReceiveId);
            if (chatRoom == null)
            {
                chatRoom = new ChatRoom() { ChatRoomId = Guid.NewGuid(), FirstUserId = request.UserSendId, SecondUserId = request.UserReceiveId };
                await _chatRoomRepository.AddChatRoom(chatRoom);
            };
            var userInvoke = await _userDataClient.GetUserById(request.UserSendId);
            foreach (var link in request.mediaFiles)
            {
                var chatMessage = new ChatMessage()
                {
                    ChatMessageId = Guid.NewGuid(),
                    ChatRoomId = chatRoom.ChatRoomId,
                    Message = null,
                    MediaLink = link,
                    UserId = request.UserSendId,
                    SendDate = DateTime.Now,
                    Type = "Media"
                };
                await _chatMessageRepository.AddAsync(chatMessage);
            }

            await _messageBusClient.PublishNewNotification(new NotificationMessageDTO()
            {
                UserId = request.UserReceiveId,
                UserInvoke = request.UserSendId,
                EventType = "NewMessage",
                Message = $"{userInvoke.NickName} sent you a message"
            });

            Console.WriteLine("Published new notification to message bus");

            return Ok("Sent media files successfully");
        }
    }
}
