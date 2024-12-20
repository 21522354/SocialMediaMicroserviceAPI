using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer.DTOs;
using PostService.Data_Layer.Models;
using PostService.SyncDataService;
using System.ComponentModel.Design;

namespace PostService.Data_Layer.Repository
{
    public class PostRepository : Repository<Post, Guid>, IPostRepository
    {
        private readonly PostServiceDBContext _context;
        private readonly IUserDataClient _userDataClient;
        private readonly IMapper _mapper;

        public PostRepository(PostServiceDBContext context, IUserDataClient userDataClient, IMapper mapper) : base(context) 
        {
            _context = context;
            _userDataClient = userDataClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Post>> GetAllPostAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<IEnumerable<Post>> GetPostsByUserId(Guid id)
        {
            var listPost = await _context.Posts.Where(p => p.UserId == id).Include(p => p.PostComments).Include(p => p.PostMedias).Include(p => p.PostLikes).Include(p => p.PostHagtags).ToListAsync();
            return listPost;
        }

        public async Task<IEnumerable<Post>> GetRandomPost(Guid userId)
        {
            var listUserFollowing = await _userDataClient.GetUserFollowing(userId);
            var listUserGuid = listUserFollowing.Select(p => p.UserId).ToList();

            var listPost = await _context.Posts
                .Where(p => p.UserId != userId && !listUserGuid.Contains(p.UserId))
                .Include(p => p.PostComments).Include(p => p.PostLikes).Include(p => p.PostHagtags)
                .Include(p => p.PostMedias)
                .ToListAsync();

            return listPost;
        }

        public async Task<IEnumerable<Post>> GetReels(Guid userId)
        {
            var listReel = await _context.Posts
                .Where(p => p.IsReel == true && p.UserId != userId)
                .ToListAsync();

            var listSeenReels = await _context.SeenReels
                .Where(p => p.UserId == userId)
                .Select(p => p.PostId)
                .ToListAsync();

            var unseenReels = listReel
                .Where(p => !listSeenReels.Contains(p.PostId))
                .ToList();

            return unseenReels;
        }
    }
}
