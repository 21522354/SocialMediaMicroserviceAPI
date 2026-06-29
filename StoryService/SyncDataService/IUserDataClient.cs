using StoryService.Data_Layer.DTOs;

namespace StoryService.SyncDataService
{
    public interface IUserDataClient
    {
        Task<UserReadDTO> GetUserById(int id);
        Task<List<UserReadDTO>> GetUsersFollowing(int userId);
    }
}
