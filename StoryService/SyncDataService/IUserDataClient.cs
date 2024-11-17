using StoryService.Data_Layer.DTOs;

namespace StoryService.SyncDataService
{
    public interface IUserDataClient
    {
        Task<UserReadDTO> GetUserById(Guid id);
        Task<List<UserReadDTO>> GetUsersFollowing(Guid userId);
    }
}
