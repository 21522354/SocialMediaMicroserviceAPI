using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;

        public StoryController(IStoryRepository storyRepository,
            IUserAlreadySeenStoryRepository userAlreadySeenStoryRepository, 
            IUserDataClient userDataClient,
            IMapper mapper)
        {
            _storyRepository = storyRepository;
            _userAlreadySeenStoryRepository = userAlreadySeenStoryRepository;
            _userDataClient = userDataClient;
            _mapper = mapper;
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
        [HttpPost("alreadySeen")]
        public async Task<IActionResult> CreateUserAlreadySeenStory([FromBody]CreateUserAlreadySeenStoryRequest request)
        {
            var userAlreadySeen = _mapper.Map<UserAlreadySeenStory>(request);
            await _userAlreadySeenStoryRepository.CreateUserAlreadySeenStory(userAlreadySeen);
            return Ok("Seen story successfully");
        }
        [HttpPost]
        public async Task<IActionResult> CreateStory([FromBody]CreateStoryRequest request)
        {
            var newStory = _mapper.Map<Story>(request);
            newStory.StoryId = Guid.NewGuid();
            newStory.CreatedDate = DateTime.Now;
            await _storyRepository.CreateStory(newStory);
            return Ok(newStory.StoryId);        
        }
        [HttpGet("usersadasdffdfdas")]
        public async Task<IActionResult> get()
        {
            return Ok(await _userAlreadySeenStoryRepository.GetAll());      
        }
        
    }
}
