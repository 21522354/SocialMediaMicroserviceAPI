using StoryService.Data_Layer.Models;

namespace StoryService.Data_Layer.Repository
{
    public interface IUserAlreadySeenStoryRepository
    {
        Task<IEnumerable<UserAlreadySeenStory>> GetByStoryId(Guid storyId);
        Task<UserAlreadySeenStory> GetByBothId(Guid storyId, int userId);
        Task CreateUserAlreadySeenStory(UserAlreadySeenStory story);
        Task<IEnumerable<UserAlreadySeenStory>> GetAll();       

    }
}
