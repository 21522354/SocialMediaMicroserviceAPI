using Microsoft.EntityFrameworkCore;
using StoryService.Data_Layer.Models;
using StoryService.SyncDataService;

namespace StoryService.Data_Layer.Repository
{
    public class UserAlreadySeenStoryRepository : IUserAlreadySeenStoryRepository
    {
        private readonly StoryServiceDBContext _context;
        private readonly IUserDataClient _userDataClient;

        public UserAlreadySeenStoryRepository(StoryServiceDBContext context, IUserDataClient userDataClient)
        {
            _context = context;
            _userDataClient = userDataClient;
        }
        public async Task CreateUserAlreadySeenStory(UserAlreadySeenStory story)
        {
            var existStory = await _context.UserAlreadySeenStories.Where(p => p.UserId == story.UserId && p.StoryId == story.StoryId).FirstOrDefaultAsync();
            if (existStory != null)
            {
                return;
            }
            await _context.UserAlreadySeenStories.AddAsync(story);
            await _context.SaveChangesAsync();
        }

        public async Task<UserAlreadySeenStory> GetByBothId(Guid storyId, Guid userId)
        {
            var story = await _context.UserAlreadySeenStories.Where(p => p.UserId == userId && p.StoryId == storyId).FirstOrDefaultAsync();
            if(story == null)
            {
                throw new Exception("Can't find this entity");
            }
            return story;
        }

        public async Task<IEnumerable<UserAlreadySeenStory>> GetByStoryId(Guid storyId)
        {
            var stories = await _context.UserAlreadySeenStories.Where(p => p.StoryId == storyId).ToListAsync();
            return stories;
        }
    }
}
