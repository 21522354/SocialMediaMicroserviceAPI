using UserService.DataLayer.Models;

namespace UserService.DataLayer.Repository
{
    public interface IUserFollowRepository : IRepository<UserFollow, int>
    {
        Task<UserFollow> GetBySelfIdAndFollowId(Guid SelfId, Guid FollowId);  
    }
}
