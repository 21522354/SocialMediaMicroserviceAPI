using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public class PostLikeRepository : IPostLikeRepository
    {
        private PostServiceDBContext _context;

        public PostLikeRepository(PostServiceDBContext context)
        {
            _context = context;
        }

        public async Task AddPostLikeAsync(PostLike postLike)
        {
            await _context.PostLikes.AddAsync(postLike);   
            await _context.SaveChangesAsync();  
        }

        public async Task DeletePostLikeAsync(PostLike postLike)
        {
            _context.PostLikes.Remove(postLike);
            await _context.SaveChangesAsync();
        }

        public async Task<PostLike> FindPostLike(Guid postId, Guid userId)
        {
            return await _context.PostLikes.FirstOrDefaultAsync(p => p.PostId == postId && p.UserId == userId);
        }

        public async Task<IEnumerable<PostLike>> GetLikeForPost(Guid postId)
        {
            var listPostLike = await _context.PostLikes.Where(p => p.PostId == postId).ToListAsync();
            return listPostLike;    
        }

    }
}
