using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoryService.AsyncDataService;
using StoryService.Data_Layer;
using StoryService.Data_Layer.DTOs;
using StoryService.Data_Layer.Models;
using StoryService.Data_Layer.Repository;
using StoryService.SyncDataService;

namespace StoryService.Controllers
{
    [Route("api/stories")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IUserAlreadySeenStoryRepository _userAlreadySeenStoryRepository;
        private readonly IUserDataClient _userDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IMapper _mapper;
        private readonly StoryServiceDBContext _context;

        public StoryController(IStoryRepository storyRepository,
            IUserAlreadySeenStoryRepository userAlreadySeenStoryRepository,
            IUserDataClient userDataClient,
            IMessageBusClient messageBusClient,
            IMapper mapper,
            StoryServiceDBContext context)
        {
            _storyRepository = storyRepository;
            _userAlreadySeenStoryRepository = userAlreadySeenStoryRepository;
            _userDataClient = userDataClient;
            _messageBusClient = messageBusClient;
            _mapper = mapper;
            _context = context;
        }
        [HttpGet("{storyId}")]
        public async Task<IActionResult> GetStoryById(Guid storyId)
        {
            var story = await _storyRepository.GetByIdAsync(storyId);
            var user = await _userDataClient.GetUserById(story.UserId);
            var stories = new List<Story>();
            stories.Add(story);
            return Ok(_mapper.Map<StoryReadDTO>((stories, user)));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _storyRepository.GetAllAsync());
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetFriendStories(Guid userId)
        {
            var listFriends = await _userDataClient.GetUsersFollowing(userId);
            var friendsStories = new List<StoryReadDTO>();
            foreach (var friend in listFriends)
            {
                var stories = await _storyRepository.GetFriendStory(friend.UserId);
                if (stories.Count() == 0) continue;
                int count = 0;
                foreach (var story in stories)
                {
                    var userAlreadySeen = await _userAlreadySeenStoryRepository.GetByBothId(story.StoryId, userId);
                    if (userAlreadySeen != null) count++;
                }
                var friendStories = _mapper.Map<StoryReadDTO>((stories, friend));
                friendStories.Index = count;
                friendsStories.Add(friendStories);
            }
            return Ok(friendsStories);
        }
        [HttpGet("user/savedStories/{userId}")]
        public async Task<IActionResult> GetSavedStoriesByUserId(Guid userId)
        {
            var user = await _userDataClient.GetUserById(userId);
            var stories = await _storyRepository.GetSavedStories(userId);
            var storyRead = _mapper.Map<StoryReadDTO>((stories, user));
            storyRead.Index = 0;
            return Ok(storyRead);
        }
        [HttpPost("alreadySeen")]
        public async Task<IActionResult> CreateUserAlreadySeenStory([FromBody] CreateUserAlreadySeenStoryRequest request)
        {
            var userAlreadySeen = _mapper.Map<UserAlreadySeenStory>(request);
            await _userAlreadySeenStoryRepository.CreateUserAlreadySeenStory(userAlreadySeen);
            return Ok("Seen story successfully");
        }
        [HttpPost("markSaved")]
        public async Task<IActionResult> MarkSavedStory(MarkSaveStoryDTO request)
        {
            var story = await _context.Stories.FindAsync(request.StoryId);
            if(story == null)
            {
                return BadRequest("Can't find this story");
            }
            story.IsSaved = true;
            _context.SaveChanges();
            return Ok("This story is saved");
        }
        [HttpDelete("{storyId}")]
        public async Task<IActionResult> DeleteStory(Guid storyId)
        {
            var story = await _context.Stories.FindAsync(storyId);
            if (story == null)
            {
                return BadRequest("Can't find this story");
            }
            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();
            return Ok("Delete story successfully");
        }
        [HttpPost]
        public async Task<IActionResult> CreateStory([FromBody]CreateStoryRequest request)
        {
            var newStory = _mapper.Map<Story>(request);
            var user = await _userDataClient.GetUserById(request.UserId); 
            newStory.StoryId = Guid.NewGuid();
            newStory.CreatedDate = DateTime.Now;
            await _storyRepository.CreateStory(newStory);
            await _messageBusClient.PublishNewNotification(new NotificationMessageDTO()
            {
                UserInvoke = newStory.UserId,
                StoryId = newStory.StoryId,
                EventType = "NewStory",
                Message = $"{user.NickName} created a new story",
            });
            return Ok(new {storyId = newStory.StoryId});        
        }
        
    }
}
