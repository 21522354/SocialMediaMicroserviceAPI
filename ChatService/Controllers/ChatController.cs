using ChatService.DataLayer.DTO;
using ChatService.Service;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("users/{userId}/chatRooms")]
        public async Task<IActionResult> GetAllChatRoomForUser(int userId)
        {
            return Ok(await _chatService.GetAllChatRoomForUser(userId));
        }

        [HttpGet("chatRooms/{chatRoomId}")]
        public async Task<IActionResult> GetSpecificChatRoom(Guid chatRoomId)
        {
            return Ok(await _chatService.GetSpecificChatRoom(chatRoomId));
        }

        [HttpPost("sendText")]
        public async Task<IActionResult> SendText(SendTextRequest request)
        {
            return Ok(await _chatService.SendText(request));
        }

        [HttpDelete("{chatRoomId}")]
        public async Task<IActionResult> DeleteChatRoom(Guid chatRoomId)
        {
            return Ok(await _chatService.DeleteChatRoom(chatRoomId));
        }

        [HttpPost("sendMedia")]
        public async Task<IActionResult> SendMedias(SendMediaRequest request)
        {
            return Ok(await _chatService.SendMedias(request));
        }
    }
}
