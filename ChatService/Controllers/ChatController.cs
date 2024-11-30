using ChatService.DataLayer.DTO;
using ChatService.DataLayer.Repository;
using ChatService.SyncDataService;
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

        public ChatController(IChatRoomRepository chatRoomRepository,
            IChatMessageRepository chatMessageRepository,
            IUserDataClient userDataClient,
            IMapper mapper)
        {
            _chatRoomRepository = chatRoomRepository;
            _chatMessageRepository = chatMessageRepository;
            _userDataClient = userDataClient;
            _mapper = mapper;
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
                listChatRoomReadDTO.Add(_mapper.Map<ChatRoomReadDTO>((friendReadDTO, room)));
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
        
    }
}
