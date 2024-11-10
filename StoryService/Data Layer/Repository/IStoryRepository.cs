using StoryService.Data_Layer.Models;

namespace StoryService.Data_Layer.Repository
{
    public interface IStoryRepository
    {
        Task<List<Story>> GetAllAsync();
        Task<Story> GetByIdAsync(Guid storyId);
        Task<List<Story>> GetFriendStory(Guid userId);
        Task<Story> CreateStory(Story story);
    }
}
