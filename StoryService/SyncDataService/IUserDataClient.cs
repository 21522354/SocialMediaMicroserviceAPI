using StoryService.Data_Layer.DTOs;

namespace StoryService.SyncDataService
{
    public interface IUserDataClient
    {
        Task<UserReadDTO> GetUserById(Guid id);
    }
}
