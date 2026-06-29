using Microsoft.AspNetCore.Mvc;
using StoryService.Data_Layer.DTOs;
using StoryService.Service;

namespace StoryService.Controllers
{
    [Route("api/stories")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;

        public StoryController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        [HttpGet("{storyId}")]
        public async Task<IActionResult> GetStoryById(Guid storyId)
        {
            return Ok(await _storyService.GetStoryById(storyId));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _storyService.GetAll());
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetFriendStories(int userId)
        {
            return Ok(await _storyService.GetFriendStories(userId));
        }

        [HttpGet("self/{userId}")]
        public async Task<IActionResult> GetSelfStory(int userId)
        {
            return Ok(await _storyService.GetSelfStory(userId));
        }

        [HttpGet("user/savedStories/{userId}")]
        public async Task<IActionResult> GetSavedStoriesByUserId(int userId)
        {
            return Ok(await _storyService.GetSavedStoriesByUserId(userId));
        }

        [HttpPost("alreadySeen")]
        public async Task<IActionResult> CreateUserAlreadySeenStory([FromBody] CreateUserAlreadySeenStoryRequest request)
        {
            return Ok(await _storyService.CreateUserAlreadySeenStory(request));
        }

        [HttpPost("markSaved")]
        public async Task<IActionResult> MarkSavedStory(MarkSaveStoryDTO request)
        {
            return Ok(await _storyService.MarkSavedStory(request));
        }

        [HttpDelete("{storyId}")]
        public async Task<IActionResult> DeleteStory(Guid storyId)
        {
            return Ok(await _storyService.DeleteStory(storyId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateStory([FromBody] CreateStoryRequest request)
        {
            return Ok(await _storyService.CreateStory(request));
        }
    }
}
