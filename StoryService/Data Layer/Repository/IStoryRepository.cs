using StoryService.Data_Layer.Models;

namespace StoryService.Data_Layer.Repository
{
    public interface IStoryRepository
    {
        Task<IEnumerable<Story>> GetAllAsync();
        Task<Story> GetByIdAsync(Guid storyId);
        Task<IEnumerable<Story>> GetFriendStory(Guid userId);
        Task<Story> CreateStory(Story story);
    }
}
