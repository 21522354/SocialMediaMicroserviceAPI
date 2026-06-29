using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using StoryService.AsyncDataService;
using StoryService.Common;
using StoryService.Data_Layer;
using StoryService.Data_Layer.DTOs;
using StoryService.Data_Layer.Models;
using StoryService.Data_Layer.Repository;
using StoryService.SyncDataService;

namespace StoryService.Service
{
    public interface IStoryService
    {
        Task<ResponseData<StoryReadDTO>> GetStoryById(Guid storyId);
        Task<ResponseData<IEnumerable<Story>>> GetAll();
        Task<ResponseData<List<StoryReadDTO>>> GetFriendStories(int userId);
        Task<ResponseData<StoryReadDTO>> GetSelfStory(int userId);
        Task<ResponseData<StoryReadDTO>> GetSavedStoriesByUserId(int userId);
        Task<ResponseData<string>> CreateUserAlreadySeenStory(CreateUserAlreadySeenStoryRequest request);
        Task<ResponseData<string>> MarkSavedStory(MarkSaveStoryDTO request);
        Task<ResponseData<string>> DeleteStory(Guid storyId);
        Task<ResponseData<object>> CreateStory(CreateStoryRequest request);
    }

    public class S_Story : IStoryService
    {
        private readonly StoryServiceDBContext _context;
        private readonly IStoryRepository _storyRepository;
        private readonly IUserAlreadySeenStoryRepository _userAlreadySeenStoryRepository;
        private readonly IUserDataClient _userDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IMapper _mapper;

        public S_Story(
            StoryServiceDBContext context,
            IStoryRepository storyRepository,
            IUserAlreadySeenStoryRepository userAlreadySeenStoryRepository,
            IUserDataClient userDataClient,
            IMessageBusClient messageBusClient,
            IMapper mapper)
        {
            _context = context;
            _storyRepository = storyRepository;
            _userAlreadySeenStoryRepository = userAlreadySeenStoryRepository;
            _userDataClient = userDataClient;
            _messageBusClient = messageBusClient;
            _mapper = mapper;
        }

        public async Task<ResponseData<StoryReadDTO>> GetStoryById(Guid storyId)
        {
            var res = new ResponseData<StoryReadDTO>();
            try
            {
                var story = await _context.Stories.AsNoTracking().FirstOrDefaultAsync(x => x.StoryId == storyId);
                if (story == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this story";
                    return res;
                }

                var user = await _userDataClient.GetUserById(story.UserId);
                res.data = _mapper.Map<StoryReadDTO>((new List<Story> { story }, user));
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

        public async Task<ResponseData<IEnumerable<Story>>> GetAll()
        {
            var res = new ResponseData<IEnumerable<Story>>();
            try
            {
                res.data = await _context.Stories.AsNoTracking().ToListAsync();
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

        public async Task<ResponseData<List<StoryReadDTO>>> GetFriendStories(int userId)
        {
            var res = new ResponseData<List<StoryReadDTO>>();
            try
            {
                var listFriends = await _userDataClient.GetUsersFollowing(userId);
                var friendsStories = new List<StoryReadDTO>();

                foreach (var friend in listFriends)
                {
                    var stories = await _storyRepository.GetFriendStory(friend.UserId);
                    if (!stories.Any()) continue;

                    var seenCount = 0;
                    foreach (var story in stories)
                    {
                        var userAlreadySeen = await _userAlreadySeenStoryRepository.GetByBothId(story.StoryId, userId);
                        if (userAlreadySeen != null) seenCount++;
                    }

                    var friendStories = _mapper.Map<StoryReadDTO>((stories, friend));
                    friendStories.Index = seenCount;
                    friendsStories.Add(friendStories);
                }

                res.data = friendsStories;
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

        public async Task<ResponseData<StoryReadDTO>> GetSelfStory(int userId)
        {
            var res = new ResponseData<StoryReadDTO>();
            try
            {
                var user = await _userDataClient.GetUserById(userId);
                var stories = await _storyRepository.GetFriendStory(userId);
                var selfStories = _mapper.Map<StoryReadDTO>((stories, user));
                selfStories.Index = 0;

                res.data = selfStories;
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

        public async Task<ResponseData<StoryReadDTO>> GetSavedStoriesByUserId(int userId)
        {
            var res = new ResponseData<StoryReadDTO>();
            try
            {
                var user = await _userDataClient.GetUserById(userId);
                var stories = await _storyRepository.GetSavedStories(userId);
                var storyRead = _mapper.Map<StoryReadDTO>((stories, user));
                storyRead.Index = 0;

                res.data = storyRead;
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

        public async Task<ResponseData<string>> CreateUserAlreadySeenStory(CreateUserAlreadySeenStoryRequest request)
        {
            var res = new ResponseData<string>();
            try
            {
                var story = await _context.Stories.AsNoTracking().FirstOrDefaultAsync(x => x.StoryId == request.StoryId);
                if (story == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this story";
                    return res;
                }

                var userAlreadySeen = _mapper.Map<UserAlreadySeenStory>(request);
                await _userAlreadySeenStoryRepository.CreateUserAlreadySeenStory(userAlreadySeen);

                res.data = "Seen story successfully";
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

        public async Task<ResponseData<string>> MarkSavedStory(MarkSaveStoryDTO request)
        {
            var res = new ResponseData<string>();
            try
            {
                var story = await _context.Stories.FindAsync(request.StoryId);
                if (story == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this story";
                    return res;
                }

                story.IsSaved = true;
                await _context.SaveChangesAsync();

                res.data = "This story is saved";
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

        public async Task<ResponseData<string>> DeleteStory(Guid storyId)
        {
            var res = new ResponseData<string>();
            try
            {
                var story = await _context.Stories.FindAsync(storyId);
                if (story == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this story";
                    return res;
                }

                _context.Stories.Remove(story);
                await _context.SaveChangesAsync();

                res.data = "Delete story successfully";
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

        public async Task<ResponseData<object>> CreateStory(CreateStoryRequest request)
        {
            var res = new ResponseData<object>();
            try
            {
                var user = await _userDataClient.GetUserById(request.UserId);
                if (user == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this user";
                    return res;
                }

                var newStory = _mapper.Map<Story>(request);
                newStory.StoryId = Guid.NewGuid();
                newStory.CreatedDate = DateTime.Now;
                await _storyRepository.CreateStory(newStory);

                await PublishNotificationAsync(new NotificationMessageDTO
                {
                    UserInvoke = newStory.UserId,
                    StoryId = newStory.StoryId,
                    EventType = "NewStory",
                    Message = $"{user.NickName} created a new story",
                });

                res.data = new { storyId = newStory.StoryId };
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
