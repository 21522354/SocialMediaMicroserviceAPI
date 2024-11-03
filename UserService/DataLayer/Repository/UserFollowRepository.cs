using Microsoft.EntityFrameworkCore;
using UserService.DataLayer.Models;

namespace UserService.DataLayer.Repository
{
    public class UserFollowRepository : Repository<UserFollow, int>, IUserFollowRepository
    {
        private readonly UserDBContext _context;
        public UserFollowRepository(UserDBContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<UserFollow> GetBySelfIdAndFollowId(Guid SelfId, Guid FollowId)
        {
            return await _context.Follows.FirstOrDefaultAsync(p => p.UserFromId == SelfId && p.UserToId == FollowId);
        }
    }
}
