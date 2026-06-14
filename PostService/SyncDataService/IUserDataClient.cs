using PostService.Data_Layer.DTOs;

namespace PostService.SyncDataService
{
    public interface IUserDataClient
    {
        Task<UserReadDTO> GetUserById(int id);
        Task<List<UserReadDTO>> GetUserFollower(int id);    
        Task<List<UserReadDTO>> GetUserFollowing(int id);  
    }
}
