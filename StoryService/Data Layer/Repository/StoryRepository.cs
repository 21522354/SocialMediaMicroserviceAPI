using Microsoft.EntityFrameworkCore;
using StoryService.Data_Layer.Models;
using StoryService.SyncDataService;

namespace StoryService.Data_Layer.Repository
{
    public class StoryRepository : IStoryRepository
    {
        private readonly StoryServiceDBContext _context;
        private readonly IUserDataClient _userDataClient;

        public StoryRepository(StoryServiceDBContext context, IUserDataClient userDataClient)
        {
            _context = context;
            _userDataClient = userDataClient;
        }

        public async Task<Story> CreateStory(Story story)
        {
            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();
            return story;   
        }

        public async Task<List<Story>> GetAllAsync()
        {
            return await _context.Stories.ToListAsync();
        }

        public async Task<Story> GetByIdAsync(Guid storyId)
        {
            var story = await _context.Stories.FindAsync(storyId);
            if(story == null)
            {
                throw new Exception("Can't find this story");
            }
            return story;
        }

        public async Task<List<Story>> GetFriendStory(Guid userId)
        {
            var friendStories = await _context.Stories
            .Where(p => p.UserId == userId && p.CreatedDate > DateTime.Now.AddHours(-24))
            .OrderBy(p => p.CreatedDate)
            .ToListAsync();
            return friendStories;
        }

        public async Task<List<Story>> GetSavedStories(Guid userId)
        {
            var stories = await _context.Stories
                .Where(p => p.UserId == userId && p.IsSaved == true)
                .OrderBy(p => p.CreatedDate)
                .ToListAsync();
            return stories; 
        }
    }
}
