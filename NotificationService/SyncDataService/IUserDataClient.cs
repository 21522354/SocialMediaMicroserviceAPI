using NotificationService.DataLayer.DTOs;

namespace NotificationService.SyncDataService
{
    public interface IUserDataClient
    {
        Task<UserReadDTO> GetUserById(int id);
        Task<UserReadDTO> GetUserByNickName(string nickName);   
        Task<List<UserReadDTO>> GetUserFollower(int id);
    }
}
