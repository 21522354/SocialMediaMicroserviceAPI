using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public class PostHagtagRepository : IPostHagtagRepository
    {
        private readonly PostServiceDBContext _context;

        public PostHagtagRepository(PostServiceDBContext context)
        {
            _context = context;
        }
        public async Task AddAsync(PostHagtag postHagtag)
        {
            await _context.AddAsync(postHagtag);    
            await _context.SaveChangesAsync();  
        }

        public async Task<List<PostHagtag>> GetPostHagtagsByName(string name)
        {
            var listPostWithHagtagName = await _context.PostHagtags
                .Where(p => p.HagtagName == name)
                .Include(p => p.Post)
                .Include(p => p.Post.PostComments)
                .Include(p => p.Post.PostLikes)
                .Include(p => p.Post.PostMedias)
                .Include(p => p.Post.PostHagtags)
                .ToListAsync();
            return listPostWithHagtagName;
        }

        public async Task<List<PostHagtag>> GetPostHagtagsByPost(Guid postId)
        {
            var listPostHagtagForPost = await _context.PostHagtags.Where(p => p.PostId == postId).ToListAsync();
            return listPostHagtagForPost;
        }

        public async Task<List<PostHagtag>> GetRelatedHagtag(string name)
        {
            var listRelatedHagtag = await _context.PostHagtags.Where(p => p.HagtagName.ToLower().Contains(name.ToLower())).ToListAsync();
            return listRelatedHagtag;
        }
    }
}
