using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public class UnseenPostReposiroty : IUnseenPostRepository
    {
        private readonly PostServiceDBContext _context;

        public UnseenPostReposiroty(PostServiceDBContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(UnseenPost unseenPost)
        {
            await _context.UnseenPosts.AddAsync(unseenPost);
            await _context.SaveChangesAsync();  
        }

        public async Task DeleteAsync(UnseenPost unseenPost)
        {
            _context.Remove(unseenPost);
            await _context.SaveChangesAsync();
        }

        public async Task<UnseenPost> GetByBothId(Guid postId, Guid userId)
        {
            return await _context.UnseenPosts.Where(p => p.PostId == postId && p.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UnseenPost>> GetUnseenPostByUserId(Guid userId)
        {
            var listUnseenPost = await _context.UnseenPosts.Where(p => p.UserId == userId).ToListAsync();
            return listUnseenPost;      
        }
    }
}
