using ChatService.DataLayer.Repository;
using ChatService.SyncDataService;
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

        public ChatController(IChatRoomRepository chatRoomRepository,
            IChatMessageRepository chatMessageRepository,
            IUserDataClient userDataClient)
        {
            _chatRoomRepository = chatRoomRepository;
            _chatMessageRepository = chatMessageRepository;
            _userDataClient = userDataClient;
        }
        [HttpGet("users/{userId}/chatRooms")]
        public async Task<IActionResult> GetAllChatRoomForUser(Guid userId)
        {
            var user = await _userDataClient.GetUserById(userId);   
            if(user == null)
            {
                return BadRequest("Can't find this user");
            }

            return Ok(await _chatRoomRepository.GetALlChatRoomForUser(userId)); 
        }
    }
}
