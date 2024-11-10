using StoryService.Data_Layer.DTOs;

namespace StoryService.SyncDataService
{
    public interface IUserDataClient
    {
        Task<UserReadDTO> GetUserById(Guid id);
        Task<IEnumerable<UserReadDTO>> GetUsersFollowing(Guid userId);
    }
}
