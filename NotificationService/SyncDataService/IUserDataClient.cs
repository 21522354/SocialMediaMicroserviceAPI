using NotificationService.DataLayer.DTOs;

namespace NotificationService.SyncDataService
{
    public interface IUserDataClient
    {
        Task<UserReadDTO> GetUserById(Guid id);
        Task<UserReadDTO> GetUserByNickName(string nickName);   
        Task<List<UserReadDTO>> GetUserFollower(Guid id);
    }
}
