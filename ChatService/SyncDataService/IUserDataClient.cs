using ChatService.DataLayer.DTO;

namespace ChatService.SyncDataService
{
    public interface IUserDataClient
    {
        Task<UserReadDTO> GetUserById(Guid userId); 
    }
}
